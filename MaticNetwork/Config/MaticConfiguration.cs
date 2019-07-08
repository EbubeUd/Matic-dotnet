using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Config
{
    public class MaticConfiguration
    {
        //Holds the chain id
        const Chain chain = Nethereum.Signer.Chain.Ropsten;

        public const string DepositManagerAddress = "0x4072fab2a132bf98207cbfcd2c341adb904a67e9";
        public const string RootChainAddress = "0x60e2b19b9a87a3f37827f2c8c8306be718a5f9b4";
        public const string WithdrawManagerAddress = "0x4ef2b60cdd4611fa0bc815792acc14de4c158d22";
        public const string ParentProvider = "https://ropsten.infura.io/v3/07cfeed6a3ba46e3b061705e01fc1d76";
        public const string SyncerUrl = "https://matic-syncer2.api.matic.network/api/v1";
        public const string WatcherUrl = "https://ropsten-watcher2.api.matic.network/api/v1";
        public const string Web3Provider = "https://ropsten-watcher2.api.matic.network/api/v1";
    }
}
