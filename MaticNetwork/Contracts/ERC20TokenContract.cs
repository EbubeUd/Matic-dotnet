using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using MaticNetwork.Helpers;
using MaticNetwork.Models;
using MaticNetwork.Models.ContractFunctions;
using MaticNetwork.Models.ContractFunctions.ERC20Functions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;


namespace MaticNetwork.Contracts
{
    public class ERC20TokenContract
    {

        #region Initializers
        public const string ABI = @"[{'constant':true,'inputs':[],'name':'name','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'spender','type':'address'},{'name':'value','type':'uint256'}],'name':'approve','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'totalSupply','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'decimals','outputs':[{'name':'','type':'uint8'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'spender','type':'address'},{'name':'addedValue','type':'uint256'}],'name':'increaseAllowance','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'owner','type':'address'}],'name':'balanceOf','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'renounceOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'owner','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'isOwner','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'symbol','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'spender','type':'address'},{'name':'subtractedValue','type':'uint256'}],'name':'decreaseAllowance','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'owner','type':'address'},{'name':'spender','type':'address'}],'name':'allowance','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'newOwner','type':'address'}],'name':'transferOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'token','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'inputs':[{'name':'_token','type':'address'},{'name':'_name','type':'string'},{'name':'_symbol','type':'string'},{'name':'_decimals','type':'uint8'}],'payable':false,'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'token','type':'address'},{'indexed':true,'name':'from','type':'address'},{'indexed':true,'name':'to','type':'address'},{'indexed':false,'name':'amountOrTokenId','type':'uint256'},{'indexed':false,'name':'input1','type':'uint256'},{'indexed':false,'name':'input2','type':'uint256'},{'indexed':false,'name':'output1','type':'uint256'},{'indexed':false,'name':'output2','type':'uint256'}],'name':'LogTransfer','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'from','type':'address'},{'indexed':true,'name':'to','type':'address'},{'indexed':false,'name':'value','type':'uint256'}],'name':'Transfer','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'owner','type':'address'},{'indexed':true,'name':'spender','type':'address'},{'indexed':false,'name':'value','type':'uint256'}],'name':'Approval','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'token','type':'address'},{'indexed':true,'name':'from','type':'address'},{'indexed':false,'name':'amountOrTokenId','type':'uint256'},{'indexed':false,'name':'input1','type':'uint256'},{'indexed':false,'name':'output1','type':'uint256'}],'name':'Deposit','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'token','type':'address'},{'indexed':true,'name':'from','type':'address'},{'indexed':false,'name':'amountOrTokenId','type':'uint256'},{'indexed':false,'name':'input1','type':'uint256'},{'indexed':false,'name':'output1','type':'uint256'}],'name':'Withdraw','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'previousOwner','type':'address'},{'indexed':true,'name':'newOwner','type':'address'}],'name':'OwnershipTransferred','type':'event'},{'constant':false,'inputs':[{'name':'user','type':'address'},{'name':'amount','type':'uint256'}],'name':'deposit','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'amount','type':'uint256'}],'name':'withdraw','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'to','type':'address'},{'name':'value','type':'uint256'}],'name':'transfer','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'from','type':'address'},{'name':'to','type':'address'},{'name':'value','type':'uint256'}],'name':'transferFrom','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'}]";


        //Sets the Matic ERC20 Contract address as the default address but this Address can Be changed when a new instance of this class is created
        string ContractAddress = "0x70459e550254b9d3520a56ee95b78ee4f2dbd846";

        //This Contract Instance will be used for making calls only and not transactions
        Contract contract;

        //This is the url of the provider eg: https://testnet.matic.network
        string ProviderUrl;


        /// <summary>
        /// Initializes an ERC20Token contract
        /// </summary>
        /// <param name="web3"></param>
        /// <param name="tokenAddress"></param>
        public ERC20TokenContract(string providerUrl, string tokenAddress)
        {
            ProviderUrl = providerUrl;
            ContractAddress = tokenAddress;
            Web3 Web3Instance = new Web3(providerUrl);
            contract = Web3Instance.Eth.GetContract(ABI, ContractAddress);
        }

        #endregion


        #region Contract Functions

        #region Calls
        /// <summary>
        /// Returns the ERC 20 Balance of an Address 
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
        #endregion

        #region Transactions
        /// <summary>
        /// The transfer function is used to Perform Transfers from one Address to another
        /// </summary>
        /// <param name="from">Source Address and Signer</param>
        /// <param name="To">Destination Address</param>
        /// <param name="Value">The Amount (In wei) of tokens to be transfered</param>
        /// <returns></returns>
        public async Task<string> Transfer(ERC20TransferModel transferModel, MaticTransactionOptions options)
        {
            //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
            Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
            Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
            Function function = contractInstance.GetFunction("transfer");

            //Fill the options
            options = await TransactionEstimateHelper.GetTransactionEstimate(transferModel, options, function);
            string response = await function.SendTransactionAsync(options.From, new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), null, transferModel.To, (BigInteger)transferModel.Value);
            return response;
        }


        /// <summary>
        /// Withdraw
        /// </summary>
        /// <param name="amount">must be token amount in wei</param>
        /// <param name="options">Matic Transaction Options</param>
        /// <returns></returns>
        public async Task<string> Withdraw(ERC20WithdrawModel withdrawModel, MaticTransactionOptions options)
        {
            //Get the Contract instance by Creating a Web3 client from the Sender's Private Key
            Web3 web3Client = Web3ClientHelper.GetWeb3Client(ProviderUrl, options.SenderPrivateKey);
            Contract contractInstance = web3Client.Eth.GetContract(ABI, ContractAddress);
            Function function = contractInstance.GetFunction("withdraw");

            options = await TransactionEstimateHelper.GetTransactionEstimate(withdrawModel, options, function);
            string response = await function.SendTransactionAsync(options.From, new HexBigInteger(options.GasLimit), new HexBigInteger(options.GasPrice.ToString()), null, (BigInteger)withdrawModel.Amount);
            return response;
        }
        #endregion


        #endregion
    }

}
