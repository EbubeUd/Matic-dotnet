using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;
using MaticNetwork.Models;
using MaticNetwork.Models.ContractFunctions;
using MaticNetwork.Models.ContractFunctions.ERC20Functions;
using MaticNetwork.Models.ContractFunctions.ERC721Functions;
using MaticNetwork.Models.ContractFunctions.RootChainFunctions;
using MaticNetwork.Models.ContractFunctions.WithdrawalManagerFunctions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Util;

namespace MaticNetwork.Helpers
{
    public class TransactionEstimateHelper
    {
        public static async Task<MaticTransactionOptions> GetTransactionEstimate(ERC20TransferModel transferModel, MaticTransactionOptions options, Function  function)
        {
            try
            {
                //Get the Account
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, transferModel.To, transferModel.Value);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;

                return options;
            }catch(Exception ex)
            {
                throw new Exception("Could not fetch transaction estimate because " + ex.Message);
            }
           
        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(ERC20ApproveModel approveModel,  MaticTransactionOptions options,  Function function)
        {
            try
            {
                //Get the Account of the Sender
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, approveModel.Spender, (BigInteger)approveModel.Value);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;
                return options;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to Fill options because " + ex.Message);
            }
           
        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(ERC20WithdrawModel withdrawModel, MaticTransactionOptions options, Function function)
        {
            try
            {
                //Get the Account and set up the sender's Address
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, withdrawModel.Amount);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;

                return options;

            }
            catch(Exception ex)
            {
                throw new Exception("Could not Fetch the Transaction Estimate because " + ex.Message);
            }
            
        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(ERC721SafeTransferFromModel safeTransferModel, MaticTransactionOptions options, Function function)
        {

            try
            {
                //Get the Account and set up the sender's Address
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, safeTransferModel.From, safeTransferModel.To, safeTransferModel.TokenId);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;

                return options;
            } catch(Exception ex)
            {
                throw new Exception("Could not fetch the transaction estimate because " + ex.Message);
            }
          
        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(ERC721TransferFromModel transferFromModel, MaticTransactionOptions options, Function function)
        {
            try
            {
                //Get the Account and set up the sender's Address
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, transferFromModel.From, transferFromModel.To, transferFromModel.TokenId);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;

                return options;
            }
            catch(Exception ex)
            {
                throw new Exception("Could not fetch the transaction estimate because " + ex.Message);
            }
         
        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(ERC721ApproveModel approvemodel, MaticTransactionOptions options, Function function)
        {
            try
            {
                //Get the Account and set up the sender's Address
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, approvemodel.To, approvemodel.TokenId);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;

                return options;
            }
            catch(Exception ex)
            {
                throw new Exception("Could not fetch the transaction estimate because " + ex.Message);
            }
           
        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(ERC721WithdrawModel withdrawModel, MaticTransactionOptions options, Function function)
        {
            try
            {
                //Get the Account and set up the sender's Address
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, withdrawModel.TokenId);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;

                return options;
            }
            catch(Exception ex)
            {
                throw new Exception("Could not fetch the transaction estimate because " + ex.Message);
            }

        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(ProcessExitsModel processExitsModel, MaticTransactionOptions options, Function function)
        {
            try
            {
                //Get the Account and set up the sender's Address
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, processExitsModel.RootTokenAddress);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;

                return options;
            }catch(Exception ex)
            {
                throw new Exception("Could not fetch the transaction estimate because " + ex.Message);
            }
            
        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(MaticTransactionOptions options, Function function)
        {
            try
            {
                //Get the Account and set up the sender's Address
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, new HexBigInteger(options.Value.ToString()));

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;

                return options;
            }
            catch(Exception ex)
            {
                throw new Exception($"There was an error fetching the transaction estimate because {ex.Message}");
            }
         
        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(DepositModel depositModel, MaticTransactionOptions options, Function function)
        {
            try
            {
                //Get the Account of the Sender
                options.ChainId = Chain.Ropsten;
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);
                
                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Get the Gas Limit
                HexBigInteger gasLimit = new HexBigInteger(4712388);
                //HexBigInteger gasLimit = await function.EstimateGasAsync(depositModel);

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;
                return options;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Fill options because " + ex.Message);
            }

        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(DepositERC721Model depositModel, MaticTransactionOptions options, Function function)
        {
            try
            {
                //Get the Account of the Sender
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, depositModel.TokenAddress, depositModel.UserAddress, (BigInteger)depositModel.TokenId);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;
                return options;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Fill options because " + ex.Message);
            }

        }

        public static async Task<MaticTransactionOptions> GetTransactionEstimate(WithdrawBurntTokensModel withdrawModel, MaticTransactionOptions options, Function function)
        {
            try
            {
                //Get the Account of the Sender
                Account account = GetAccount(options.SenderPrivateKey, options.ChainId);

                //Get the Gas Limit
                HexBigInteger gasLimit = await function.EstimateGasAsync(account.Address, null, null, withdrawModel.HeaderNumber, withdrawModel.HeaderProof, withdrawModel.BlockNumber, withdrawModel.BlockTimeStamp, withdrawModel.TxRoot, withdrawModel.ReceiptProof, withdrawModel.Path, withdrawModel.TxBytes, withdrawModel.TxProof, withdrawModel.ReceiptBytes, withdrawModel.ReceiptProof);

                //Get the Gas Price Estimate
                GasPriceEstimator gasPriceEstimator = new GasPriceEstimator();
                GasPriceEstimate gasPriceEstimate = await gasPriceEstimator.GetRecommendedGasPriceFromNetwork();

                //Fill the options
                options.GasPrice = (decimal)gasPriceEstimate.AverageGwei;
                options.GasLimit = gasLimit;
                options.From = account.Address;
                return options;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Fill options because " + ex.Message);
            }
        }

        private static Account GetAccount(string privateKey, Chain chainId)
        {
            //Check if the sender private key has been set
            if (privateKey == null) throw new Exception("'from' is required in options or set wallet using maticObject.wallet = <private key>");

            //Get the Account Using the Private Key
            Account account = new Account(privateKey, chainId);

            //Check if the account is valid
            if (account == null) throw new Exception("An account could not be generated with the private key provided");

            return account;
        }


    }
}
