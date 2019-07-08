using System;
using System.Collections.Generic;
using System.Text;

namespace MaticNetwork.Models
{
    public class GasPriceEstimate
    {

        public double LowGwei { get; set; }
        public double FastGwei { get; set; }
        public double AverageGwei { get; set; }
    }
}
