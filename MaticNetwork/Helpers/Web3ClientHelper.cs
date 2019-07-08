using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Helpers
{
    public  class Web3ClientHelper
    {
        public static Web3 GetWeb3Client(string url, string privateKey)
        {
            Account account = new Account(privateKey);
            Web3 web3 = new Web3(account, url);
            return web3;
        }

        public static Web3 GetWeb3Client(string url)
        {
            Web3 web3 = new Web3(url);
            return web3;
        }
    }
}
