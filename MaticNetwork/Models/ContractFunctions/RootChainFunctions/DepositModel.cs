using Nethereum.Hex.HexTypes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MaticNetwork.Models.ContractFunctions.RootChainFunctions
{
    public class DepositModel
    {
        public string TokenAddress { get; set; }
        public string UserAddress { get; set; }
        public BigInteger Amount { get; set; }
    }
}
