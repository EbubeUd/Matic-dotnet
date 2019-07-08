using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

using MaticNetwork.Helpers;
using MaticNetwork.Models;
using MaticNetwork.Models.ContractFunctions;
using MaticNetwork.Models.ContractFunctions.ERC20Functions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MaticNetwork.Contracts
{
    public class StandardTokenArtifactsContract
    {

        #region Initializers
        const string ABI = @"[{'constant':true,'inputs':[],'name':'name','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'totalSupply','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'}],'name':'balances','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'decimals','outputs':[{'name':'','type':'uint8'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'name':'','type':'address'},{'name':'','type':'address'}],'name':'allowed','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'symbol','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_from','type':'address'},{'indexed':true,'name':'_to','type':'address'},{'indexed':false,'name':'_value','type':'uint256'},{'indexed':false,'name':'_data','type':'bytes'}],'name':'Transfer','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'_owner','type':'address'},{'indexed':true,'name':'_spender','type':'address'},{'indexed':false,'name':'_value','type':'uint256'}],'name':'Approval','type':'event'},{'constant':false,'inputs':[{'name':'_to','type':'address'},{'name':'_value','type':'uint256'}],'name':'transfer','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_to','type':'address'},{'name':'_value','type':'uint256'},{'name':'_data','type':'bytes'}],'name':'transfer','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_from','type':'address'},{'name':'_to','type':'address'},{'name':'_value','type':'uint256'}],'name':'transferFrom','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'_owner','type':'address'}],'name':'balanceOf','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_spender','type':'address'},{'name':'_value','type':'uint256'}],'name':'approve','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'_owner','type':'address'},{'name':'_spender','type':'address'}],'name':'allowance','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'}]";

        //This Address will be set when a new instance of this class is created
        string ContractAddress;

        //Holds the url of the Provider EG: https://testnet.matic.network
        string ProviderUrl; 



        public StandardTokenArtifactsContract(string provider, string contractAddress)
        {
            ProviderUrl = provider;
            ContractAddress = contractAddress;
        }

        #endregion


        #region Contract Functions

        #region Transactions

        /// <summary>
        /// Event
        /// </summary>
        /// <param name="rootChainAddress"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public async Task<string> Approve(ERC20ApproveModel approveModel, MaticTransactionOptions options)
        {
            try
            {
                //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
                Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
                Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
                Function function = contractInstance.GetFunction("approve");

                //Fill out th options
                options = await TransactionEstimateHelper.GetTransactionEstimate(approveModel, options, function);

                //Send the transaction And recieve a response
                string response = await function.SendTransactionAsync(options.From,new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), null, approveModel.Spender,(BigInteger) approveModel.Value);

                return response;
            }
            catch(Exception ex)
            {
                throw new Exception("There was Error Performing Approval because " + ex.Message);
            }
            
        }


        #endregion

        #endregion
    }
}
