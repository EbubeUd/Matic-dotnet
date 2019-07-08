using Nethereum.Hex.HexTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models.ContractFunctions.ERC20Functions
{
    public class ERC20ApproveModel
    {
        public string Spender { get; set; }
        public HexBigInteger Value { get; set; }
    }
}
