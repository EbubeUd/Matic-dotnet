using Nethereum.Hex.HexTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MaticNetwork.Models.ContractFunctions.ERC20Functions
{
    public class ERC20TransferModel
    {
        public string To { get; set; }
        public BigInteger Value { get; set; }
    }
}
