using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models
{
    public class MaticInitOptions
    {
        public string SyncerUrl { get; set; }
        public string WatcherUrl { get; set; }
        public string MaticProvider { get; set; }
        public string ParentProvider { get; set; }
        public string RootChainAddress { get; set; }
        public string MaticWethAddress { get; set; }
        public string WithdrawManagerAddress { get; set; }
        public string DepositManagerAddress { get; set; }
    }
}
