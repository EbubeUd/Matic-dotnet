using Nethereum.Hex.HexTypes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Nethereum.RPC.Eth.DTOs;

namespace MaticNetwork.Models.Responses
{
    public abstract class TrasanctionResponse
    {
        public HexBigInteger BlockNumber { get; set; }
        public HexBigInteger BlockTimeStamp { get; set; }
        public string Root { get; set; }
        public string ParentNodes { get; set; }
        public string Path { get; set; }
    }


    public class TransactionProofResponse : TrasanctionResponse
    {
       
        public Transaction Value { get; set; }
      
    }

    public class TransactionReceiptResponse : TrasanctionResponse
    {
        public TransactionReceipt Value { get; set; }
    }
}
