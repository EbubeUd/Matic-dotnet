using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using MaticNetwork.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using MaticNetwork.Models.ContractFunctions.WithdrawalManagerFunctions;
using MaticNetwork.Helpers;

namespace MaticNetwork.Contracts
{
    public class WithdrawalManagerContract
    {
        #region Initializers
        public const string ABI = @"[{'constant':true,'inputs':[{'name':'_token','type':'address'},{'name':'_owner','type':'address'},{'name':'_tokenId','type':'uint256'}],'name':'getExitId','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'roundType','outputs':[{'name':'','type':'bytes32'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'exits','outputs':[{'name':'owner','type':'address'},{'name':'token','type':'address'},{'name':'amountOrTokenId','type':'uint256'},{'name':'burnt','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'wethToken','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'bytes32'}],'name':'ownerExits','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'depositManager','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'renounceOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'voteType','outputs':[{'name':'','type':'bytes1'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'_token','type':'address'}],'name':'getNextExit','outputs':[{'name':'','type':'uint256'},{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'owner','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'isOwner','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'networkId','outputs':[{'name':'','type':'bytes'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'rootChain','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'CHILD_BLOCK_INTERVAL','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'chain','outputs':[{'name':'','type':'bytes32'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'exitsQueues','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'_utxoPos','type':'uint256'}],'name':'getExit','outputs':[{'name':'','type':'address'},{'name':'','type':'address'},{'name':'','type':'uint256'},{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'newRootChain','type':'address'}],'name':'changeRootChain','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'newOwner','type':'address'}],'name':'transferOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'exitNFTContract','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'anonymous':false,'inputs':[{'indexed':true,'name':'user','type':'address'},{'indexed':true,'name':'token','type':'address'},{'indexed':false,'name':'amount','type':'uint256'}],'name':'Withdraw','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'exitor','type':'address'},{'indexed':true,'name':'utxoPos','type':'uint256'},{'indexed':true,'name':'token','type':'address'},{'indexed':false,'name':'amount','type':'uint256'}],'name':'ExitStarted','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'previousRootChain','type':'address'},{'indexed':true,'name':'newRootChain','type':'address'}],'name':'RootChainChanged','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'previousOwner','type':'address'},{'indexed':true,'name':'newOwner','type':'address'}],'name':'OwnershipTransferred','type':'event'},{'constant':false,'inputs':[{'name':'_nftContract','type':'address'}],'name':'setExitNFTContract','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_depositManager','type':'address'}],'name':'setDepositManager','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_token','type':'address'}],'name':'setWETHToken','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_rootToken','type':'address'},{'name':'_childToken','type':'address'},{'name':'_isERC721','type':'bool'}],'name':'mapToken','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_currentHeaderBlock','type':'uint256'}],'name':'finalizeCommit','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'exitId','type':'uint256'}],'name':'deleteExit','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_token','type':'address'}],'name':'processExits','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'headerNumber','type':'uint256'},{'name':'headerProof','type':'bytes'},{'name':'blockNumber','type':'uint256'},{'name':'blockTime','type':'uint256'},{'name':'txRoot','type':'bytes32'},{'name':'receiptRoot','type':'bytes32'},{'name':'path','type':'bytes'},{'name':'txBytes','type':'bytes'},{'name':'txProof','type':'bytes'},{'name':'receiptBytes','type':'bytes'},{'name':'receiptProof','type':'bytes'}],'name':'withdrawBurntTokens','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'headerNumber','type':'uint256'},{'name':'headerProof','type':'bytes'},{'name':'blockNumber','type':'uint256'},{'name':'blockTime','type':'uint256'},{'name':'txRoot','type':'bytes32'},{'name':'receiptRoot','type':'bytes32'},{'name':'path','type':'bytes'},{'name':'txBytes','type':'bytes'},{'name':'txProof','type':'bytes'},{'name':'receiptBytes','type':'bytes'},{'name':'receiptProof','type':'bytes'}],'name':'withdrawTokens','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_depositCount','type':'uint256'}],'name':'withdrawDepositTokens','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'}]";

        //The Address of the Contract
        string ContractAddress;

        //This contract instance will only be used for making calls to the  contract and not transactions
        Contract contract;

        //This is the url of the provider eg: https://testnet.matic.network
        string ProviderUrl;

        public WithdrawalManagerContract(string provider, string contractAddress)
        {
            ProviderUrl = provider;
            ContractAddress = contractAddress;
            Web3 Web3Instance = new Web3(provider);
            contract = Web3Instance.Eth.GetContract(ABI, ContractAddress);
        }

        #endregion

        #region Contract Functions

        #region Transactions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerNumber"></param>
        /// <param name="v1"></param>
        /// <param name="blockNumber"></param>
        /// <param name="blockTimeStamp"></param>
        /// <param name="txProofRoot"></param>
        /// <param name="receiptProofRoot"></param>
        /// <param name="v2"></param>
        /// <param name="txProofValue"></param>
        /// <param name="txProofParentNodes"></param>
        /// <param name="receiptProofValue"></param>
        /// <param name="receiptProofParentNodes"></param>
        /// <returns></returns>
        public async Task<string> WithdrawBurntTokens(WithdrawBurntTokensModel withdrawModel, MaticTransactionOptions options)
        {
            //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
            Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
            Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
            Function function = contractInstance.GetFunction("withdrawBurntTokens");

            options = await TransactionEstimateHelper.GetTransactionEstimate(withdrawModel, options, function);

            string response = await function.SendTransactionAsync(options.From, options.GasLimit, options.GasPrice, null, withdrawModel.HeaderNumber, withdrawModel.HeaderProof, withdrawModel.BlockNumber, withdrawModel.BlockTimeStamp, withdrawModel.TxRoot, withdrawModel.ReceiptRoot, withdrawModel.Path, withdrawModel.TxBytes, withdrawModel.TxProof, withdrawModel.ReceiptBytes, withdrawModel.ReceiptProof);
            return response;
        }


        /// <summary>
      
        /// </summary>
        /// <param name="rootTokenAddress"></param>11
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> ProcessExits(ProcessExitsModel processExitsModel, MaticTransactionOptions options)
        {

            //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
            Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
            Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
            Function function = contractInstance.GetFunction("processExits");

            options = await TransactionEstimateHelper.GetTransactionEstimate(processExitsModel, options, function);
            string response = await function.SendTransactionAsync(options.From, options.GasLimit, options.GasPrice, null, processExitsModel.RootTokenAddress);
            return response;
        }

        #endregion

        #endregion
    }
}