using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models.ContractFunctions.RootChainFunctions
{
    public class DepositERC721Model
    {
        public string TokenAddress { get; set; }
        public string UserAddress { get; set; }
        public int TokenId { get; set; }
    }
}
