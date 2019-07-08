using MaticNetwork.Helpers;
using MaticNetwork.Models;
using Nethereum.Contracts;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MaticNetwork.Contracts
{
    public class DepositManagerContract
    {

        #region Initializers
        public const string ABI = @"[{'constant':true,'inputs':[],'name':'roundType','outputs':[{'name':'','type':'bytes32'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'depositCount','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'reverseTokens','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'wethToken','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'renounceOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'voteType','outputs':[{'name':'','type':'bytes1'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'owner','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'isOwner','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'networkId','outputs':[{'name':'','type':'bytes'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'rootChain','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'CHILD_BLOCK_INTERVAL','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'deposits','outputs':[{'name':'header','type':'uint256'},{'name':'owner','type':'address'},{'name':'token','type':'address'},{'name':'amountOrTokenId','type':'uint256'},{'name':'createdAt','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'chain','outputs':[{'name':'','type':'bytes32'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'isERC721','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'tokens','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'newRootChain','type':'address'}],'name':'changeRootChain','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'newOwner','type':'address'}],'name':'transferOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[],'payable':false,'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_user','type':'address'},{'indexed':true,'name':'_token','type':'address'},{'indexed':false,'name':'_amountOrTokenId','type':'uint256'},{'indexed':false,'name':'_depositCount','type':'uint256'}],'name':'Deposit','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'previousRootChain','type':'address'},{'indexed':true,'name':'newRootChain','type':'address'}],'name':'RootChainChanged','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'previousOwner','type':'address'},{'indexed':true,'name':'newOwner','type':'address'}],'name':'OwnershipTransferred','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_rootToken','type':'address'},{'indexed':true,'name':'_childToken','type':'address'}],'name':'TokenMapped','type':'event'},{'constant':false,'inputs':[{'name':'_nftContract','type':'address'}],'name':'setExitNFTContract','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_token','type':'address'}],'name':'setWETHToken','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_rootToken','type':'address'},{'name':'_childToken','type':'address'},{'name':'_isERC721','type':'bool'}],'name':'mapToken','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_currentHeaderBlock','type':'uint256'}],'name':'finalizeCommit','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'currentHeaderBlock','type':'uint256'}],'name':'nextDepositBlock','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'_depositCount','type':'uint256'}],'name':'depositBlock','outputs':[{'name':'_header','type':'uint256'},{'name':'_owner','type':'address'},{'name':'_token','type':'address'},{'name':'_amountOrTokenId','type':'uint256'},{'name':'_createdAt','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_currentHeaderBlock','type':'uint256'},{'name':'_token','type':'address'},{'name':'_user','type':'address'},{'name':'_amountOrTokenId','type':'uint256'}],'name':'createDepositBlock','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'}]";

        string ContractAddress;

        Web3 Web3Instance;

        Contract contract;

        public  Contract GetContract(Web3 web3)
        {
            return contract;
        }

        public DepositManagerContract(Web3 web3, string contractAddress)
        {
            Web3Instance = web3;
            ContractAddress = contractAddress;
            contract = Web3Instance.Eth.GetContract(ABI, ContractAddress);
        }

        #endregion


        #region Contract Functions

        #region Calls
        /// <summary>
        /// Get Matic token address mapped with main chain tokenAddress.<\n>
        /// This function Returns the index of a mapped token address if it exists and<\n>
        /// Maps the token address to the contract and returns the index if the address does not exist on the contract
        /// </summary>
        /// <param name="tokenAddress">tokenAddress must be valid token address</param>
        /// <returns></returns>
        public async Task<string> GetMappedTokenAddressAsync(string tokenAddress)
        {
            object[] functionParams = new object[1] { tokenAddress };
            Function function = contract.GetFunction("tokens");
            string mappedAddress = await function.CallAsync<string>(functionParams);
            return mappedAddress;
        }

        #endregion

       

        #endregion
    }
}
