using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using MaticNetwork.Helpers;
using MaticNetwork.Models;
using MaticNetwork.Models.ContractFunctions.ERC721Functions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MaticNetwork.Contracts
{
    public class ERC721TokenContract
    {


        #region Initializers

        public const string ABI = @"[{'constant':true,'inputs':[],'name':'name','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'spender','type':'address'},{'name':'value','type':'uint256'}],'name':'approve','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'totalSupply','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'decimals','outputs':[{'name':'','type':'uint8'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'spender','type':'address'},{'name':'addedValue','type':'uint256'}],'name':'increaseAllowance','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'owner','type':'address'}],'name':'balanceOf','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'renounceOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'owner','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'isOwner','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'symbol','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'spender','type':'address'},{'name':'subtractedValue','type':'uint256'}],'name':'decreaseAllowance','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'owner','type':'address'},{'name':'spender','type':'address'}],'name':'allowance','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'newOwner','type':'address'}],'name':'transferOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'token','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'inputs':[{'name':'_token','type':'address'},{'name':'_name','type':'string'},{'name':'_symbol','type':'string'},{'name':'_decimals','type':'uint8'}],'payable':false,'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'token','type':'address'},{'indexed':true,'name':'from','type':'address'},{'indexed':true,'name':'to','type':'address'},{'indexed':false,'name':'amountOrTokenId','type':'uint256'},{'indexed':false,'name':'input1','type':'uint256'},{'indexed':false,'name':'input2','type':'uint256'},{'indexed':false,'name':'output1','type':'uint256'},{'indexed':false,'name':'output2','type':'uint256'}],'name':'LogTransfer','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'from','type':'address'},{'indexed':true,'name':'to','type':'address'},{'indexed':false,'name':'value','type':'uint256'}],'name':'Transfer','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'owner','type':'address'},{'indexed':true,'name':'spender','type':'address'},{'indexed':false,'name':'value','type':'uint256'}],'name':'Approval','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'token','type':'address'},{'indexed':true,'name':'from','type':'address'},{'indexed':false,'name':'amountOrTokenId','type':'uint256'},{'indexed':false,'name':'input1','type':'uint256'},{'indexed':false,'name':'output1','type':'uint256'}],'name':'Deposit','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'token','type':'address'},{'indexed':true,'name':'from','type':'address'},{'indexed':false,'name':'amountOrTokenId','type':'uint256'},{'indexed':false,'name':'input1','type':'uint256'},{'indexed':false,'name':'output1','type':'uint256'}],'name':'Withdraw','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'previousOwner','type':'address'},{'indexed':true,'name':'newOwner','type':'address'}],'name':'OwnershipTransferred','type':'event'},{'constant':false,'inputs':[{'name':'user','type':'address'},{'name':'amount','type':'uint256'}],'name':'deposit','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'amount','type':'uint256'}],'name':'withdraw','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'to','type':'address'},{'name':'value','type':'uint256'}],'name':'transfer','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'from','type':'address'},{'name':'to','type':'address'},{'name':'value','type':'uint256'}],'name':'transferFrom','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'}]";

        //Sets the Matic ERC20 Contract address as the default address but this Address can Be changed when a new instance of this class is created
        string ContractAddress;

        //This Contract Instance will be used for making calls only and not transactions
        Contract contract;

        //This is the url of the provider eg: https://testnet.matic.network
        string ProviderUrl;


        public ERC721TokenContract(string provider, string tokenAddress)
        {
            ProviderUrl = provider;
            Web3 Web3Instance = new Web3(provider);
            ContractAddress = tokenAddress;
            contract = Web3Instance.Eth.GetContract(ABI, ContractAddress);
        }

        #endregion

        #region Contract Functions

        #region Calls

        /// <summary>
        /// Returns the ERC721 Balance of an Address 
        /// </summary>
        /// <param name="OwnerAddress"></param>
        /// <returns></returns>
        public async Task<BigInteger> BalanceOf(string OwnerAddress)
        {
            object[] paramObjects = new object[1] { OwnerAddress };
            Function balanceOfFunction = contract.GetFunction("balanceOf");
            BigInteger balance = await balanceOfFunction.CallAsync<BigInteger>(paramObjects);
            return balance;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">The Owner Address</param>
        /// <param name="index">The Index Id</param>
        /// <returns>int</returns>
        public async Task<int> GetTokenOfOwnerByIndex(string address, int index)
        {
            try
            {
                object[] paramObjects = new object[2] { address, index };
                Function function = contract.GetFunction("tokenOfOwnerByIndex");
                int tokenId = await function.CallAsync<int>(paramObjects);
                return tokenId;
            }
            catch(Exception ex)
            {
                throw new Exception($"Could not Get Token of Owner by index because: {ex.Message}");
            }
            
        }


        #endregion

        #region Transactions



        /// <summary>
        /// Deposit ERC721 token into Matic chain.(older ERC721 or some newer contracts will not support this.
        /// In that case, first call `approveERC721TokenForDeposit` and `depositERC721Tokens`)
        /// </summary>
        /// <param name="rootChainAddress"></param>
        /// <param name="tokenId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> SafeTransferFrom(ERC721SafeTransferFromModel safeTransferModel, MaticTransactionOptions options)
        {
            //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
            Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
            Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
            Function function = contractInstance.GetFunction("SafeTransferFrom");

            options = await TransactionEstimateHelper.GetTransactionEstimate(safeTransferModel, options, function);
            string transactionHash = await function.SendTransactionAsync(options.From, new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), null, safeTransferModel.From, safeTransferModel.To, safeTransferModel.TokenId);
            return transactionHash;
        }

        /// <summary>
        /// Approve ERC721 token for deposit
        /// </summary>
        /// <param name="tokenAddress"></param>
        /// <param name="tokenId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> Approve(ERC721ApproveModel approveModel, MaticTransactionOptions options)
        {
            //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
            Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
            Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
            Function function = contractInstance.GetFunction("approve");

            options = await TransactionEstimateHelper.GetTransactionEstimate(approveModel, options, function);
            string response = await function.SendTransactionAsync(options.From, new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), null, approveModel.To, approveModel.TokenId);
            return response;
        }


        /// <summary>
        /// Transfer ERC721 Tokens From a Contract
        /// </summary>
        /// <param name="from">The Source Address</param>
        /// <param name="to">The Destination Address</param>
        /// <param name="tokenId">The Token Id</param>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> TransferFrom(ERC721TransferFromModel transferFromModel, MaticTransactionOptions options)
        {
            //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
            Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
            Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
            Function function = contractInstance.GetFunction("transferFrom");

            options = await TransactionEstimateHelper.GetTransactionEstimate(transferFromModel, options, function);
            string response = await function.SendTransactionAsync(options.From, new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), null, transferFromModel.From, transferFromModel.To, transferFromModel.TokenId);
            return response;
        }


        /// <summary>
        /// Withdraw
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> Withdraw(ERC721WithdrawModel withdrawModel, MaticTransactionOptions options)
        {
            //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
            Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
            Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
            Function function = contractInstance.GetFunction("withdraw");

            options = await TransactionEstimateHelper.GetTransactionEstimate(withdrawModel, options, function);
            string response = await function.SendTransactionAsync(options.From, new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), null, withdrawModel.TokenId);
            return response;
        }


        #endregion


        #endregion

                                                            

    }
}
