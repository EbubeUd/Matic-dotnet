using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Nethereum.Signer;

namespace MaticNetwork.Models
{
    public class MaticTransactionOptions
    {
        public bool UseParent { get; set; }
        public string From { get; set; }
        public decimal Value { get; set; }
        public BigInteger GasLimit { get; set; }
        public decimal GasPrice { get; set; }
        public int Nonce { get; set; }
        public Chain ChainId { get; set; }
        public string SenderPrivateKey { get; set; }
        public string To { get; set; }
    }
}
