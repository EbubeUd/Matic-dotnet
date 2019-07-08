using System;
using System.Collections.Generic;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;
using MaticNetwork.Contracts;
using System.Threading.Tasks;
using MaticNetwork.Models;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System.Net.Http;
using System.Net;
using MaticNetwork.Models.Responses;
using Newtonsoft.Json;
using MaticNetwork.Models.Requests;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.Eth.Blocks;
using Nethereum.JsonRpc.Client;
using MaticNetwork.Lib;
using MaticNetwork.Models.ContractFunctions.ERC20Functions;
using MaticNetwork.Models.ContractFunctions.WithdrawalManagerFunctions;
using MaticNetwork.Models.ContractFunctions.ERC721Functions;
using MaticNetwork.Models.ContractFunctions.RootChainFunctions;
using MaticNetwork.Helpers;

namespace MaticNetwork
{
    /// <summary>
    /// Author: Ebube Ud
    /// </summary>
    public class Matic
    {
        #region Initializations
        string ParentWeb3ProviderUrl;
        string MaticWeb3ProviderUrl;

        Web3 web3;
        Web3 ParentWeb3;

        RootChainContract maticRootChainContract;
        WithdrawalManagerContract maticWithrawalManagerContract;
        DepositManagerContract maticDepositManagerContract;
        ERC20TokenContract maticERC20Contract;
        ERC721TokenContract maticERC721Contract;

        string ERC20ContractAddress;
        string RootChainContractAddress;
        string WithdrawManagerContractAddress;
        string DepositManagerContractAddress;
        string MaticWethAddress;

        string SyncerUrl;
        string WatcherUrl;

        //Internal Cache
        static Dictionary<string, string> TokenMappedCache = new Dictionary<string, string>();


        public Matic(MaticInitOptions options)
        {
            //Assign the Web3 Provider Urls
            ParentWeb3ProviderUrl = options.ParentProvider;
            MaticWeb3ProviderUrl = options.MaticProvider;

            //Define the Addresses For Matic
            RootChainContractAddress = options.RootChainAddress;
            WithdrawManagerContractAddress = options.WithdrawManagerAddress;
            DepositManagerContractAddress = options.DepositManagerAddress;

            //Define the Web3 Instances
            web3 = new Web3(MaticWeb3ProviderUrl);
            ParentWeb3 = new Web3(ParentWeb3ProviderUrl);

            //Create Root Chain Contract
            maticRootChainContract = new RootChainContract(ParentWeb3ProviderUrl, RootChainContractAddress);

            //Create Withdrawal Manger Contract
            maticWithrawalManagerContract = new WithdrawalManagerContract(MaticWeb3ProviderUrl, WithdrawManagerContractAddress);

            //Create Deposit Manager Contract
            maticDepositManagerContract = new DepositManagerContract(web3, DepositManagerContractAddress);

            //Assign the Syncer and Watcher Url
            SyncerUrl = options.SyncerUrl;
            WatcherUrl = options.WatcherUrl;

        }

        //Holds the Private Key of the Sender
        public string Wallet;

        //Holds the Address of the Sender
        public string WalletAddress;

        #endregion

        #region Actions

        /// <summary>
        /// get matic token address mapped with mainchain tokenAddress.
        /// Tested But not yet sure of the response
        /// </summary>
        /// <param name="tokenAddress">tokenAddress must be valid token address</param>
        /// <returns>String</returns>
        public async Task<string> GetMappedTokenAddress(string tokenAddress)
        {
            //Convert the string to lower case characters
            string addressToLowerCase = tokenAddress.ToLower();

            //check if the address is existing in the internal Cache
            if (!TokenMappedCache.ContainsKey(tokenAddress))
            {
                //Fetch mapped address from the contract
                string mappedTokenAddress = await maticDepositManagerContract.GetMappedTokenAddressAsync(tokenAddress);
                if(mappedTokenAddress != null)TokenMappedCache[tokenAddress] = mappedTokenAddress;
            }
            return TokenMappedCache[addressToLowerCase];
        }


        /// <summary>
        /// Get ERC721 Token Balance
        /// </summary>
        /// <param name="userAddress">userAddress holds the address of the user</param>
        /// <param name="tokenAddress">tokenAddress holds the address of the contract</param>
        /// <param name="options">options holds the options to be passed for the transaction.</param>
        /// <returns>BigInteger</returns>
        public async Task<BigInteger> BalanceOfERC721(string userAddress, string tokenAddress, MaticTransactionOptions options)
        {
            try
            {

                string web3ProviderUrl = MaticWeb3ProviderUrl;

                //Set The web3 Object to the Parent Web3 Object if UseParent is set to true
                if (options.UseParent) web3ProviderUrl = ParentWeb3ProviderUrl;

                //Get the ERC721 Contract using the token address of the contract
                ERC721TokenContract erc721Contract = new ERC721TokenContract(web3ProviderUrl, tokenAddress);

                //Get the Balance of the User from the Contract
                BigInteger balance = await erc721Contract.BalanceOf(userAddress);

                return balance;
            }
            catch(Exception ex)
            {
                throw new Exception($"Could not fetch ERC721 balance for ");
            }

            
            
        }


        /// <summary>
        /// Gets the ERC721 Token Of the Owner(address) using the index Id
        /// </summary>
        /// <param name="address">The Address of the owner</param>
        /// <param name="tokenAddress">THe Address of the Contract</param>
        /// <param name="index">The index Id</param>
        /// <param name="options">Options for the transaction</param>
        /// <returns></returns>
        public async Task<int> TokenOfOwnerByIndexERC721(string address, string tokenAddress, int index, MaticTransactionOptions options)
        {
            try
            {
                string web3ProviderUrl = MaticWeb3ProviderUrl;

                //Set The web3 Object to the Parent Web3 Object if UseParent is set to true
                if (options.UseParent) web3ProviderUrl = ParentWeb3ProviderUrl;

                //Get the ERC721 Contract using the token address of the contract
                ERC721TokenContract erc721Contract = new ERC721TokenContract(MaticWeb3ProviderUrl, tokenAddress);

                //Get the TokenId from the Contract
                int TokenId = await erc721Contract.GetTokenOfOwnerByIndex(address, index);

                return TokenId;
            }
            catch(Exception ex)
            {
                throw new Exception($"Could not fetch token by index because: {ex.Message}");
            }
           

        }


        /// <summary>
        /// Deposit Ethers
        /// </summary>
        /// <param name="options">Options for the transaction</param>
        /// <returns></returns>
        public async Task<string> DepositEthers(MaticTransactionOptions options)
        {
            //Check if the options is correctly set
            if(options == null) throw new Exception("Parameters required for the transaction on Matic Network are missing");
            if ((options != null) && (options.From == null)) throw new Exception("Parameters required for the transaction on Matic Network are missing");

            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");

            string transactionResponse = await maticRootChainContract.DepositEthers(options);
            return transactionResponse;
        }



        /// <summary>
        /// Approves Tokens for Deposit
        /// </summary>
        /// <param name="tokenAddress">Must be a valid ERC20 contract Address</param>
        /// <param name="Amount">Amount of tokens to be approved</param>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns>string</returns>
        public async Task<string> ApproveERC20TokensForDeposit(string tokenAddress, BigInteger Amount, MaticTransactionOptions options)
        {
            //Check if the options is correctly set
            if ((options != null) &&  (tokenAddress == null) )
            {
                throw new Exception("Parameters requred for the transaction on Matic Network are missing");
            }

            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");


            //Get the Standard Token Artifacts Contract using the token address of the contract
            StandardTokenArtifactsContract standardTokenArtifacts = new StandardTokenArtifactsContract(MaticWeb3ProviderUrl, tokenAddress);

            //Build the model for Approve
            ERC20ApproveModel model = new ERC20ApproveModel()
            {
                Spender = tokenAddress,
                Value = new HexBigInteger(Amount)
            };

            //Approve Tokens
            string response = await standardTokenArtifacts.Approve(model, options);

            return response;
        }


        /// <summary>
        /// Deposit ERC20 Tokens
        /// </summary>
        /// <param name="tokenAddress">The Token Address</param>
        /// <param name="userAddress">The Address of the User</param>
        /// <param name="amount">Amount to Deposit</param>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> DepositErc20Tokens(string tokenAddress, string userAddress, BigInteger amount, MaticTransactionOptions options)
        {
            //check if the options are correctly set
            if ((options != null) && ( String.IsNullOrEmpty(options.From)  || String.IsNullOrEmpty(tokenAddress)) )
            {
                throw new Exception("Parameters required for the transaction on Matic Network are missing");
            }

            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");


            //Deposit ERC20 Tokens
            DepositModel depositModel = new DepositModel()
            {
                TokenAddress = tokenAddress,
                UserAddress = userAddress,
                Amount = amount
            };
            string response = await maticRootChainContract.Deposit(depositModel, options);

            return response;
        }


        /// <summary>
        /// Deposit ERC721 token into Matic chain.(older ERC721 or some newer contracts will not support this.
        /// In that case, first call `approveERC721TokenForDeposit` and `depositERC721Tokens`)
        /// </summary>
        /// <param name="rootChainAddress"></param>
        /// <param name="tokenId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> SafeDepositERC721Tokens(string tokenAddress, int? tokenId, MaticTransactionOptions options)
        {
            //check if the options are correctly set
            if ((options != null) && (String.IsNullOrEmpty(options.From) || String.IsNullOrEmpty(tokenAddress) || tokenId == null))
            {
                throw new Exception("Parameters required for the transaction on Matic Network are missing");
            }

            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");


            //Get the contract using the Token Address
            ERC721TokenContract erc721Contract = new ERC721TokenContract(ParentWeb3ProviderUrl, tokenAddress);
            ERC721SafeTransferFromModel safeTransferModel = new ERC721SafeTransferFromModel()
            {
                From = options.From,
                To = tokenAddress,
                TokenId = tokenId.Value
            };

            string response = await erc721Contract.SafeTransferFrom(safeTransferModel, options);

            return response;
        }


        /// <summary>
        /// Approve ERC721 token for deposit
        /// </summary>
        /// <param name="tokenAddress">Must be a Valid ERC721 Token Address</param>
        /// <param name="tokenId">Mapped Token Id</param>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> ApproveERC721TokenForDeposit(string tokenAddress, int? tokenId, MaticTransactionOptions options = null)
        {
            //check if the options are correctly set
            if ((options != null) && (String.IsNullOrEmpty(options.From) || String.IsNullOrEmpty(tokenAddress) || tokenId == null))
            {
                throw new Exception("Parameters required for the transaction on Matic Network are missing");
            }


            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");


            //Get the Contract using the token Address
            ERC721TokenContract erc721Contract = new ERC721TokenContract(ParentWeb3ProviderUrl, tokenAddress);
            ERC721ApproveModel approveModel = new ERC721ApproveModel()
            {
                To = tokenAddress,
                TokenId = tokenId.Value
            };

            string response = await erc721Contract.Approve(approveModel, options);
            return response;
        }



        /// <summary>
        /// Deposit ERC721 Tokens
        /// </summary>
        /// <param name="tokenAddress">The Address of the Contract</param>
        /// <param name="userAddress">The Address of the User</param>
        /// <param name="tokenId">The Token Id</param>
        /// <param name="options">The Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> DepositERC721Tokens(string tokenAddress, string userAddress, int? tokenId, MaticTransactionOptions options = null)
        {
            //check if the options are correctly set
            if ((options != null) && (String.IsNullOrEmpty(options.From) || String.IsNullOrEmpty(tokenAddress) || tokenId == null))
            {
                throw new Exception("Parameters required for the transaction on Matic Network are missing");
            }

            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");


            DepositERC721Model depositModel = new DepositERC721Model()
            {
                TokenAddress = tokenAddress,
                UserAddress = userAddress,
                TokenId = tokenId.Value
            };

            //Make the deposit
            string response = await maticRootChainContract.DepositERC721(depositModel, options);

            return response;

        }



        /// <summary>
        /// The transfer function is used to Perform Transfers from one Address to another
        /// </summary>
        /// <param name="tokenAddress">Source Address and Signer</param>
        /// <param name="userAddress">Address of the reciepient</param>
        /// <param name="amount">The Amount (In wei) of tokens to be transfered to the reciepient</param>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> TransferTokens(string tokenAddress, string userAddress, BigInteger amount, MaticTransactionOptions options)
        {
            //check if the options are correctly set
            if ((options != null) && (String.IsNullOrEmpty(options.From) || String.IsNullOrEmpty(tokenAddress) || amount == null))
            {
                throw new Exception("Parameters required for the transaction on Matic Network are missing");
            }

            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");


            string web3Provider = MaticWeb3ProviderUrl;

            //Set The web3 Object to the Parent Web3 Object if UseParent is set to true
            if (options.UseParent) web3Provider = ParentWeb3ProviderUrl;

            //Get the contract using the token Address
            ERC20TokenContract erc20TokenContract = new ERC20TokenContract(web3Provider, tokenAddress);

            //Make the Transfer
            ERC20TransferModel transferModel = new ERC20TransferModel()
            {
                To = userAddress,
                Value = new HexBigInteger(amount)
            };
            string response = await erc20TokenContract.Transfer(transferModel, options);

            return response;
        }


        /// <summary>
        /// TransferERC721Tokens
        /// </summary>
        /// <param name="tokenAddress">Address of the Token's Contract</param>
        /// <param name="userAddress">Address of the User</param>
        /// <param name="tokenId">The Index of the Token on Matic</param>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> TransferERC721Tokens(string tokenAddress, string userAddress, int? tokenId, MaticTransactionOptions options)
        {
            Web3 web3Object = web3;

            //Use the ParentWeb3 if UseParent is set to true
            if (options.UseParent) web3Object = ParentWeb3;

            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");


            //Get the Contract with the token Address
            ERC721TokenContract erc721TokenContract = new ERC721TokenContract(MaticWeb3ProviderUrl, tokenAddress);

            //Send the Transaction
            ERC721TransferFromModel transferModel = new ERC721TransferFromModel()
            {
                From = userAddress,
                To = tokenAddress,
                TokenId = tokenId.Value
            };

            string response = await erc721TokenContract.TransferFrom(transferModel, options);

            return response;
        }


        /// <summary>
        /// Transfer Ethers
        /// Tested
        /// </summary>
        /// <param name="to">Address of the reciepient</param>
        /// <param name="amount">Amount of Ethers to Transfer</param>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> TransferEthers(string to, BigInteger amount, MaticTransactionOptions options)
        {
            try
            {
                //check if the options are correctly set
                if ((options != null) && (String.IsNullOrEmpty(options.From)  || amount == null))
                {
                    throw new Exception("Parameters required for the transaction on Matic Network are missing");
                }

                //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
                if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
                if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");

                // if matic chain, transfer normal WETH tokens
                if (!options.UseParent) return await TransferTokens(MaticWethAddress, to, amount, options);

                //Send the Transaction using the parent web3
                Web3 clientWeb3 = Web3ClientHelper.GetWeb3Client(ParentWeb3ProviderUrl, options.SenderPrivateKey);
                TransactionReceipt response = await clientWeb3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(to, 0.11m); ;

                return response.TransactionHash;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }


        /// <summary>
        /// Start withdraw process with given amount for token.
        /// </summary>
        /// <param name="tokenAddress">Must be a valid ERC20 Token Address</param>
        /// <param name="amount">must be token amount in wei</param>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> StartWithdraw(string tokenAddress, BigInteger amount, MaticTransactionOptions options)
        {
            //check if the options are correctly set
            if ((options != null) && (String.IsNullOrEmpty(options.From) || String.IsNullOrEmpty(options.SenderPrivateKey) || String.IsNullOrEmpty(tokenAddress) || amount == null))
            {
                throw new Exception("Parameters required for the transaction on Matic Network are missing");
            }

            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");


            //Get the Contract Using the token Address
            ERC20TokenContract erc20Contract = new ERC20TokenContract(MaticWeb3ProviderUrl, tokenAddress);
            ERC20WithdrawModel withdrawModel = new ERC20WithdrawModel()
            {
                Amount = amount
            };
            string response = await erc20Contract.Withdraw(withdrawModel, options);

            return response;
        }


        /// <summary>
        /// Start withdraw process with given tokenId for token.
        /// </summary>
        /// <param name="tokenAddress">must be valid ERC721 token address</param>
        /// <param name="tokenId">must be token tokenId in wei </param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> StartERC721Withdraw(string tokenAddress, int? tokenId, MaticTransactionOptions options)
        {
            //check if the options are correctly set
            if ((options != null) && (String.IsNullOrEmpty(options.From) || String.IsNullOrEmpty(tokenAddress) || tokenId == null))
            {
                throw new Exception("Parameters required for the transaction on Matic Network are missing");
            }

            //If the Private Key was not sent in the options assign the one set in the wallet. If both are null, throw an exception
            if (string.IsNullOrEmpty(options.SenderPrivateKey)) options.SenderPrivateKey = Wallet;
            if (String.IsNullOrEmpty(options.SenderPrivateKey)) throw new Exception("Please provide the private Key first, using 'Matic.Wallet = <PrivateKey>'");


            //Get the Contract using the tokenAddress
            ERC721TokenContract erc721TokenContract = new ERC721TokenContract(MaticWeb3ProviderUrl, tokenAddress);
            ERC721WithdrawModel withdrawalModel = new ERC721WithdrawModel()
            {
                To = tokenAddress,
                TokenId = tokenId.Value
            };

            string response = await erc721TokenContract.Withdraw(withdrawalModel, options);
            return response;
        }


        /// <summary>
        /// Get transaction object using txId from Matic chain.
        /// </summary>
        /// <param name="transactionId">must be valid tx id</param>
        /// <returns></returns>
        public async Task<Transaction> GetTx(string transactionId)
        {
            if(SyncerUrl != null)
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(SyncerUrl);
                    HttpResponseMessage response = await client.GetAsync($"tx/{transactionId}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string contentString = await response.Content.ReadAsStringAsync();
                        TransactionResponse transactionResponse = JsonConvert.DeserializeObject<TransactionResponse>(contentString);
                        Transaction txn = JsonConvert.DeserializeObject<Transaction>(transactionResponse.Reciept);
                        return txn;
                    }
                }catch(Exception ex)
                {
                    //ignore the error
                }
            }
            EthGetTransactionByHash transactionByHash =  ParentWeb3.Eth.Transactions.GetTransactionByHash;
            Transaction tx = await transactionByHash.SendRequestAsync(transactionId);
            return tx;
        }


        /// <summary>
        /// Get receipt object using txId from Matic chain.
        /// </summary>
        /// <param name="transactionId">must be valid tx id</param>
        /// <returns></returns>
        public async Task<TransactionReceipt> GetReciept(string transactionId)
        {
            if (SyncerUrl != null)
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(SyncerUrl);
                    HttpResponseMessage response = await client.GetAsync($"tx/{transactionId}/receipt");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string contentString = await response.Content.ReadAsStringAsync();
                        TransactionResponse transactionResponse = JsonConvert.DeserializeObject<TransactionResponse>(contentString);
                        TransactionReceipt txReceipt = JsonConvert.DeserializeObject<TransactionReceipt>(transactionResponse.Reciept);
                        return txReceipt;
                    }
                }
                catch (Exception ex)
                {
                    //ignore the error
                }

            }

            EthGetTransactionReceipt transactionByHash = ParentWeb3.Eth.Transactions.GetTransactionReceipt;
            TransactionReceipt tx = await transactionByHash.SendRequestAsync(transactionId);
            return tx;
        }


        /// <summary>
        /// Get Transaction Proof
        /// </summary>
        /// <param name="trancationId"></param>
        /// <returns></returns>
        public async Task<TransactionProofResponse> GetTxProof(string transactionId)
        {
            if (!String.IsNullOrEmpty(SyncerUrl))
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(SyncerUrl);
                    HttpResponseMessage response = await client.GetAsync($"tx/{transactionId}/proof");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string contentString = await response.Content.ReadAsStringAsync();
                        TransactionProofResponse transactionProofResponse = JsonConvert.DeserializeObject<TransactionProofResponse>(contentString);
                        return transactionProofResponse;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"There was an error fetching the TX proof from the syncer url because {ex.Message}");
                   
                }
            }
            else
            {
                try
                {
                    EthGetTransactionByHash transactionByHash = ParentWeb3.Eth.Transactions.GetTransactionByHash;
                    Transaction tx = await transactionByHash.SendRequestAsync(transactionId);
                    TransactionProofResponse transactionProofResponse = new TransactionProofResponse()
                    {
                        Value = tx
                    };

                    return transactionProofResponse;
                }catch(Exception ex)
                {
                    throw new Exception($"There was an error fething the txProof because {ex.Message}");
                }
            }

            return null;
        }

        /// <summary>
        /// Verify Transaction Proof
        /// </summary>
        /// <param name="transactionProof"></param>
        /// <returns></returns>
        public string VerifyTxProof(string transactionProof)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Get Receipt Proof
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<TransactionReceiptResponse> GetReceiptProof(string transactionId)
        {
            if (!String.IsNullOrEmpty(SyncerUrl))
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(SyncerUrl);
                    HttpResponseMessage response = await client.GetAsync($"tx/{transactionId}/receipt/proof");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string contentString = await response.Content.ReadAsStringAsync();
                        TransactionReceiptResponse receiptProofResponse = JsonConvert.DeserializeObject<TransactionReceiptResponse>(contentString);
                        return receiptProofResponse;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not fetch the Receipt Proof from the syncer URL because {ex.Message}");
                }
            }
            else
            {
                try
                {
                    EthGetTransactionReceipt transactionByHash = ParentWeb3.Eth.Transactions.GetTransactionReceipt;
                    TransactionReceipt tx = await transactionByHash.SendRequestAsync(transactionId);
                    TransactionReceiptResponse transactionReceiptResponse = new TransactionReceiptResponse()
                    {
                        Value = tx
                    };

                    return transactionReceiptResponse;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not fetch te receipt proof because {ex.Message}");
                }
            }
           

            return null;
        }


        /// <summary>
        /// Verify Receipt Proof
        /// </summary>
        /// <returns></returns>
        public string VerifyReceiptProof()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Get Header Object
        /// </summary>
        /// <param name="blockNumber"></param>
        /// <returns></returns>
        public async Task<Header> GetHeaderObject(HexBigInteger blockNumber)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(WatcherUrl);
                HttpResponseMessage response = await client.GetAsync($"header/included/{blockNumber.Value}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contentString = await response.Content.ReadAsStringAsync();
                    Header header = JsonConvert.DeserializeObject<Header>(contentString);
                    return header;
                }
            }
            catch (Exception ex)
            {
                
                //ignore the error
            }

            return null;
        }


        /// <summary>
        /// Get Header Proof
        /// </summary>
        /// <param name="blockNumber"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public async Task<string> GetHeaderProof(HexBigInteger blockNumber, Header header)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(SyncerUrl);
                HttpResponseMessage response = await client.GetAsync($"block/{blockNumber}/proof?start={header.Start}&end={header.End}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contentString = await response.Content.ReadAsStringAsync();
                    return contentString;
                }
            }
            catch (Exception ex)
            {
        
            } 

            return "";
        }


        /// <summary>
        /// Verify Header Proof
        /// </summary>
        /// <param name="headerProof"></param>
        /// <returns></returns>
        public string VerifyHeaderProof(string headerProof)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Withdraw
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> Withdraw(string transactionId, MaticTransactionOptions options)
        {
            // fetch transaction & receipt proof
            (TransactionProofResponse txProof, TransactionReceiptResponse receiptProof) proofResponse;
            proofResponse.txProof = await GetTxProof(transactionId);
            proofResponse.receiptProof = await GetReceiptProof(transactionId);


            //fetch the header object and header proof
            Header header = null;
            try
            {
                header = await GetHeaderObject(proofResponse.txProof.BlockNumber);
            }catch(Exception ex)
            {
                //Do nothing
            }

            //Check if the Header block was found
            if (header == null) throw new Exception($"No corresponding checkpoint/header block found for {transactionId}");

            //Get the HEader Proof
            string headerProof = await GetHeaderProof(proofResponse.txProof.BlockNumber, header);

            //perform the withdrawal
            WithdrawBurntTokensModel withdrawBurntTokensModel = new WithdrawBurntTokensModel()
            {
                HeaderNumber = header.Number,
                HeaderProof = "",
                BlockNumber = proofResponse.txProof.BlockNumber,
                BlockTimeStamp = proofResponse.txProof.BlockTimeStamp,
                TxRoot = proofResponse.txProof.Root,
                ReceiptRoot = proofResponse.receiptProof.Root,
                Path = "",
                TxBytes = proofResponse.txProof.Value.ToString(),
                TxProof = proofResponse.txProof.ParentNodes,
                ReceiptBytes = proofResponse.receiptProof.Value.ToString(),
                ReceiptProof = proofResponse.receiptProof.ParentNodes
            };
            string response = await maticWithrawalManagerContract.WithdrawBurntTokens(withdrawBurntTokensModel,  options);

            return response;
        }


        /// <summary>
        /// Process Exits
        /// </summary>
        /// <param name="rootTokenAddress"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> ProcessExits(string rootTokenAddress, MaticTransactionOptions options)
        {
            ProcessExitsModel processExitsModel = new ProcessExitsModel()
            {
                RootTokenAddress = rootTokenAddress
            };
            string response = await maticWithrawalManagerContract.ProcessExits(processExitsModel, options);
            return response;
        }



        public async Task<string> WithdrawLocally(string transactionId, MaticTransactionOptions options)
        {
            EthGetTransactionByHash ethGetTransactionByHash = web3.Eth.Transactions.GetTransactionByHash;
            Transaction withdrawTransaction = await ethGetTransactionByHash.SendRequestAsync(transactionId);

            EthGetTransactionReceipt ethGetTransactionReceipt = web3.Eth.Transactions.GetTransactionReceipt;
            TransactionReceipt withdrawReceipt = await ethGetTransactionReceipt.SendRequestAsync(transactionId);

            EthGetBlockWithTransactionsByNumber ethGetBlockWithTransactionsByNumber = web3.Eth.Blocks.GetBlockWithTransactionsByNumber;
            BlockWithTransactions withdrawBlock = await ethGetBlockWithTransactionsByNumber.SendRequestAsync(withdrawReceipt.BlockNumber);


            //Draft Withdraw Object
            DraftWithdrawObject withdrawObject = new DraftWithdrawObject()
            {
                TxId = transactionId,
                Block = withdrawBlock,
                Tx = withdrawTransaction,
                Receipt = withdrawReceipt
            };

            //Get Transaction Proof
            TransactionProofResponse transactionProof = await  GetTxProof(withdrawObject.Tx, withdrawObject.Block);

            //Get Receipt Proof
            TransactionProofResponse receiptProof = await GetReceiptProof(withdrawObject.Receipt, withdrawObject.Block, web3);

            //Get Current Header Block
            HexBigInteger currentHeaderBlock = await maticRootChainContract.CurrenctHeaderBlock();

            //Get the Header
            Header header = await maticRootChainContract.HeaderBlock(new HexBigInteger(currentHeaderBlock.Value - 1));

            HexBigInteger headerNumber = new HexBigInteger(currentHeaderBlock.Value - 1);
            int start = header.Start;
            int end = header.End;

            Header headers = await GetHeaders(start, end, web3);

            MerkleTree tree = new MerkleTree(headers);

            string blockHeader = GetBlockHeader(withdrawObject.Block);

            string headerProof = await tree.GetProof(blockHeader);

            WithdrawBurntTokensModel withdrawBurntTokensModel = new WithdrawBurntTokensModel()
            {
                HeaderNumber = headerNumber,
                HeaderProof = "",
                BlockNumber = withdrawObject.Block.Number,
                BlockTimeStamp = withdrawObject.Block.Timestamp,
                TxRoot = withdrawObject.Block.TransactionsRoot,
                ReceiptRoot = withdrawObject.Block.ReceiptsRoot,
                Path = receiptProof.Path,
                TxBytes = withdrawObject.Tx.ToString(),
                TxProof = transactionProof.ParentNodes,
                ReceiptBytes = withdrawObject.Receipt.ToString(),
                ReceiptProof = receiptProof.ParentNodes
            };

            string withdrawTxObject = await maticWithrawalManagerContract.WithdrawBurntTokens(withdrawBurntTokensModel, options);

            return withdrawTxObject;
        }

        private string GetBlockHeader(BlockWithTransactions block)
        {
            throw new NotImplementedException();
        }

        private Task<Header> GetHeaders(int start, int end, Web3 web3)
        {
            throw new NotImplementedException();
        }

        private Task<TransactionProofResponse> GetReceiptProof(TransactionReceipt receipt, BlockWithTransactions block, Web3 web3)
        {
            throw new NotImplementedException();
        }

        private Task<TransactionProofResponse> GetTxProof(Transaction tx, BlockWithTransactions block)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
