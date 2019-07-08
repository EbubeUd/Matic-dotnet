using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models.ContractFunctions.ERC721Functions
{
    public class ERC721SafeTransferFromModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public int TokenId { get; set; }
    }
}

