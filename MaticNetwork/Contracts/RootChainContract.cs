using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using MaticNetwork.Models;
using MaticNetwork.Models.Requests;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MaticNetwork.Helpers;
using MaticNetwork.Models.ContractFunctions.RootChainFunctions;

namespace MaticNetwork.Contracts
{
    public class RootChainContract
    {
        #region Initializers
        //This holds the ABI of the Contract
        public const string ABI = @"[{'constant':true,'inputs':[],'name':'childChainContract','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'roundType','outputs':[{'name':'','type':'bytes32'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'headerBlocks','outputs':[{'name':'root','type':'bytes32'},{'name':'start','type':'uint256'},{'name':'end','type':'uint256'},{'name':'createdAt','type':'uint256'},{'name':'proposer','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'depositManager','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'renounceOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'stakeManager','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'voteType','outputs':[{'name':'','type':'bytes1'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'owner','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'isOwner','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'networkId','outputs':[{'name':'','type':'bytes'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'CHILD_BLOCK_INTERVAL','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'proofValidatorContracts','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'chain','outputs':[{'name':'','type':'bytes32'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'withdrawManager','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'newOwner','type':'address'}],'name':'transferOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[],'payable':false,'stateMutability':'nonpayable','type':'constructor'},{'payable':true,'stateMutability':'payable','type':'fallback'},{'anonymous':false,'inputs':[{'indexed':true,'name':'previousChildChain','type':'address'},{'indexed':true,'name':'newChildChain','type':'address'}],'name':'ChildChainChanged','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'validator','type':'address'},{'indexed':true,'name':'from','type':'address'}],'name':'ProofValidatorAdded','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'validator','type':'address'},{'indexed':true,'name':'from','type':'address'}],'name':'ProofValidatorRemoved','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'proposer','type':'address'},{'indexed':true,'name':'number','type':'uint256'},{'indexed':false,'name':'start','type':'uint256'},{'indexed':false,'name':'end','type':'uint256'},{'indexed':false,'name':'root','type':'bytes32'}],'name':'NewHeaderBlock','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'previousOwner','type':'address'},{'indexed':true,'name':'newOwner','type':'address'}],'name':'OwnershipTransferred','type':'event'},{'constant':false,'inputs':[{'name':'vote','type':'bytes'},{'name':'sigs','type':'bytes'},{'name':'extradata','type':'bytes'}],'name':'submitHeaderBlock','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'exitId','type':'uint256'}],'name':'deleteExit','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_nftContract','type':'address'}],'name':'setExitNFTContract','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_token','type':'address'}],'name':'setWETHToken','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_rootToken','type':'address'},{'name':'_childToken','type':'address'},{'name':'_isERC721','type':'bool'}],'name':'mapToken','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'newChildChain','type':'address'}],'name':'setChildContract','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_validator','type':'address'}],'name':'addProofValidator','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_validator','type':'address'}],'name':'removeProofValidator','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'currentChildBlock','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'currentHeaderBlock','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'_headerNumber','type':'uint256'}],'name':'headerBlock','outputs':[{'name':'_root','type':'bytes32'},{'name':'_start','type':'uint256'},{'name':'_end','type':'uint256'},{'name':'_createdAt','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'_depositCount','type':'uint256'}],'name':'depositBlock','outputs':[{'name':'','type':'uint256'},{'name':'','type':'address'},{'name':'','type':'address'},{'name':'','type':'uint256'},{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_stakeManager','type':'address'}],'name':'setStakeManager','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_depositManager','type':'address'}],'name':'setDepositManager','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_withdrawManager','type':'address'}],'name':'setWithdrawManager','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'depositEthers','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'_token','type':'address'},{'name':'_user','type':'address'},{'name':'_tokenId','type':'uint256'}],'name':'depositERC721','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_token','type':'address'},{'name':'_user','type':'address'},{'name':'_amount','type':'uint256'}],'name':'deposit','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_token','type':'address'},{'name':'_user','type':'address'},{'name':'_amount','type':'uint256'}],'name':'transferAmount','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_user','type':'address'},{'name':'_amount','type':'uint256'},{'name':'_data','type':'bytes'}],'name':'tokenFallback','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'','type':'uint256'}],'name':'finalizeCommit','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'slash','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'}]";

        //This Contract Instance will be used for making calls only and not transactions
        string ContractAddress;

        //This is the url of the provider eg: https://testnet.matic.network
        string ProviderUrl;

        //This Contract instance will only be used for making calls to the contract. Not Performing Transactions
        Contract contract; 


        public RootChainContract(string provider, string contractAddress)
        {
            ProviderUrl = provider;
            ContractAddress = contractAddress;
            Web3 Web3Instance = new Web3(provider);
            contract = Web3Instance.Eth.GetContract(ABI, ContractAddress);
        }

        #endregion

        #region Contract Functions

        #region Calls
        /// <summary>
        /// Get Current Header Block
        /// </summary>
        /// <returns></returns>
        public async Task<HexBigInteger> CurrenctHeaderBlock()
        {
            Function function = contract.GetFunction("currentHeaderBlock");
            HexBigInteger response = await function.CallAsync<HexBigInteger>();
            return response;
        }


        /// <summary>
        /// Get Header Block
        /// </summary>
        /// <param name="headerBlock"></param>
        /// <returns></returns>
        public async Task<Header> HeaderBlock(BigInteger headerBlock)
        {
            Function function = contract.GetFunction("headerBlock");
            Header response = await function.CallAsync<Header>();
            return response;
        }

        #endregion


        #region Transactions

        /// <summary>
        /// Deposit Ethers
        /// </summary>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> DepositEthers(MaticTransactionOptions options)
        {
            try
            {
                //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
                Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
                Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
                Function function = contractInstance.GetFunction("depositEthers");

                //Fill the options
                options = await TransactionEstimateHelper.GetTransactionEstimate(options, function);

                //Send the transaction
                string response = await function.SendTransactionAsync(options.From, new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), new HexBigInteger(options.Value.ToString()));
                return response;
            }
            catch(Exception ex)
            {
                throw new Exception($"There was an error Depositing Ethers because {ex.Message}");
            }
           
        }


        /// <summary>
        /// Deposit
        /// </summary>
        /// <param name="tokenAddress">Valid ERC20 Token</param>
        /// <param name="userAddress"></param>
        /// <param name="amount"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> Deposit(DepositModel depositModel, MaticTransactionOptions options)
        {
            try
            {
                //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
                Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
                Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
                Function function = contractInstance.GetFunction("deposit");

                //Fill the options
                options = await TransactionEstimateHelper.GetTransactionEstimate(depositModel, options, function);

                //Convert the amount from wei to Ethers
                decimal etherEquivalent = Web3.Convert.FromWei(depositModel.Amount);

                object[] paramObjects = new object[3] { depositModel.TokenAddress, depositModel.UserAddress, depositModel.Amount };

                string response = await function.SendTransactionAsync(options.From, new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), null,  depositModel.TokenAddress, depositModel.UserAddress, depositModel.Amount);
               
                return response;

            }
            catch(Exception ex)
            {
                throw new Exception($"There was an error depositing {depositModel.Amount} Ethers into {depositModel.TokenAddress} : {ex.Message}");
            }
           
        }


        /// <summary>
        /// Deposit ERC 721
        /// </summary>
        /// <param name="tokenAddress">Must be a valic ERC721 Token</param>
        /// <param name="userAddress">User Address</param>
        /// <param name="tokenId">Mapped Token Index</param>
        /// <param name="options">Matic Trnsaction Options</param>
        /// <returns></returns>
        public async Task<string> DepositERC721(DepositERC721Model depositModel, MaticTransactionOptions options)
        {
            //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
            Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
            Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
            Function function = contractInstance.GetFunction("depositERC721");

            //Fill the options
            options = await TransactionEstimateHelper.GetTransactionEstimate(depositModel, options, function);

            string response = await function.SendTransactionAsync(options.From, new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), null, depositModel.TokenAddress, depositModel.UserAddress, depositModel.TokenId);
            return response;
        }

        #endregion


        #endregion

    }
}
