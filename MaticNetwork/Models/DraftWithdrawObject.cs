using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models
{
    public class DraftWithdrawObject
    {
        public string TxId { get; set; }
        public BlockWithTransactions Block { get; set; }
        public Transaction Tx { get; set; }
        public TransactionReceipt Receipt { get; set; }
    }
}
