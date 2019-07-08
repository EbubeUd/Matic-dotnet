using Nethereum.Hex.HexTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models.ContractFunctions.WithdrawalManagerFunctions
{
    public class WithdrawBurntTokensModel
    {
        public HexBigInteger HeaderNumber { get; set; }
        public string HeaderProof { get; set; }
        public HexBigInteger BlockNumber { get; set; }
        public HexBigInteger BlockTimeStamp { get; set; }
        public string TxRoot { get; set; }
        public string ReceiptRoot { get; set; }
        public string Path { get; set; }
        public string TxBytes { get; set; }
        public string TxProof { get; set; }
        public string ReceiptBytes { get; set; }
        public string ReceiptProof { get; set; }
    }
}
