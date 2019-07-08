using MaticNetwork.Models;
using System;
using System.Numerics;
using System.Threading.Tasks;
using MaticNetwork.Config;

namespace MaticNetwork
{
    class Example
    {
        static MaticInitOptions maticInitOptions;


        static void Main(string[] args)
        {
            //Build the Matic Init Options
            maticInitOptions = new MaticInitOptions();
            maticInitOptions.DepositManagerAddress = MaticConfiguration.DepositManagerAddress;
            maticInitOptions.RootChainAddress = MaticConfiguration.RootChainAddress;
            maticInitOptions.WithdrawManagerAddress = MaticConfiguration.WithdrawManagerAddress;
            maticInitOptions.ParentProvider = MaticConfiguration.ParentProvider;
            maticInitOptions.SyncerUrl = MaticConfiguration.ParentProvider;
            maticInitOptions.WatcherUrl = MaticConfiguration.WatcherUrl;
            maticInitOptions.MaticProvider = MaticConfiguration.Web3Provider;

            //Call A function Here
            string response =  ApproveERC20TokensForDeposit().GetAwaiter().GetResult();
            Console.WriteLine(response);
            Console.ReadLine();
        }



        static async Task<string> GetMappedTokenAddress()
        {
            string tokenAddress = "0x721a441b213687c5594...";       //Replace with ERC721 token Address

            Matic matic = new Matic(maticInitOptions);
            string response = await matic.GetMappedTokenAddress(tokenAddress);

            return response;
        }


        static async Task<string> DepositErc20Tokens()
        {
          
            //Create a new Matic instance with the Init Options
            Matic matic = new Matic(maticInitOptions);
            matic.Wallet = "BD3D1BD2B1D2FAE58...";      //Replace with Private Key

            string erc20TokenAddress = "0x1ba441b213687c5594...";       //Replace with ERC20 token Address
            string userAddress = "0xb6218956F76576327DEE...";       //Replace with ERC20 token Address
            BigInteger val = 100000000000000000;

            //Initialize the transaction Options
            MaticTransactionOptions transactionOptions = new MaticTransactionOptions();
            transactionOptions.From = "address";
            transactionOptions.UseParent = true;
                   

            string response = await matic.DepositErc20Tokens(erc20TokenAddress, userAddress, val, transactionOptions);
            return response;
        }


        static async Task<BigInteger> BalanceOfERC721()
        {

            //Create a new Matic instance with the Init Options
            Matic matic = new Matic(maticInitOptions);
            matic.Wallet = "BD3D1BD2B1D2FAE58...";   //replace with private Key

            string tokenAddress = "0x721a441b213687c5594...";      //Replace with ERC721 Address
            string userAddress = "0xb6218956F76576327DEE...";      //Replace with valid ERC20 Address

            //Initialize the transaction Options
            MaticTransactionOptions transactionOptions = new MaticTransactionOptions();
            transactionOptions.From = userAddress;
            transactionOptions.UseParent = true;

            BigInteger response = await matic.BalanceOfERC721(userAddress, tokenAddress, transactionOptions);
            return response;
        }


        static async Task<string> ApproveERC20TokensForDeposit()
        {
            
            string erc20TokenAddress = "0x1ba441b213687c5594...";   //Replace with ERC20 token Address
            string userAddress = "0xb6218956F76576327DEE...";       //Replace with valid Eth Address
            BigInteger value = 10;

            Matic matic = new Matic(maticInitOptions);
            matic.Wallet = "BD3D1BD2B1D2FAE58...";    //replace with private key


            MaticTransactionOptions options = new MaticTransactionOptions();
            options.From = userAddress;
         
            string response = await matic.ApproveERC20TokensForDeposit(erc20TokenAddress, value, options);
            return response;
        }
    }
}
