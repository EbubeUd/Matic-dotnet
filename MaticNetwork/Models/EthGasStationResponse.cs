using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models
{
    public class EthGasStationResponse
    {
        public double fastWait { get; set; }
        public double average { get; set; }
        public long blockNum { get; set; }
        public double safelow_calc { get; set; }
        public double fast { get; set; }
        public double fastest { get; set; }
        public double safeLow { get; set; }
        public double safelow_txpool { get; set; }
        public double safeLowWait { get; set; }
        public decimal block_time { get; set; }
        public double average_txpool { get; set; }
        public double avgWait { get; set; }
        public decimal speed { get; set; }
        public double fastestWait { get; set; }
        public double average_calc { get; set; }

    }
}
