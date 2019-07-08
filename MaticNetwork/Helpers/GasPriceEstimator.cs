using MaticNetwork.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MaticNetwork.Helpers
{
    public class GasPriceEstimator
    {
        public async Task<GasPriceEstimate> GetRecommendedGasPriceFromNetwork()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://ethgasstation.info/");
            var response = await client.GetAsync("json/ethgasAPI.json");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string contentString = await response.Content.ReadAsStringAsync();


                EthGasStationResponse ethGasStationResponse = JsonConvert.DeserializeObject<EthGasStationResponse>(contentString);
                GasPriceEstimate gasPriceEstimate = new GasPriceEstimate
                {
                    LowGwei = ethGasStationResponse.safeLow,
                    AverageGwei = ethGasStationResponse.average,
                    FastGwei = ethGasStationResponse.fast,
                };
                return gasPriceEstimate;
            }

            throw new Exception($"Fetching Of Recommended Gas Price Returned Status Code {response.StatusCode}");

        }
    }
}
