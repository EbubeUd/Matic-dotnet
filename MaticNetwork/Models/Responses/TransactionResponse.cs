using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models.Responses
{
    public class TransactionResponse
    {
        [JsonProperty(PropertyName = "tx")]
        public string Tx { get; set; }
        public string Reciept { get; set; }
    }
}
