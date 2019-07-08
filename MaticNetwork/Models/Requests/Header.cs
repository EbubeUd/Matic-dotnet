using Nethereum.Hex.HexTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models.Requests
{
    public class Header
    {
        public int Start { get; set; }
        public int End { get; set; }
        public HexBigInteger Number { get; set; }
    }
}
