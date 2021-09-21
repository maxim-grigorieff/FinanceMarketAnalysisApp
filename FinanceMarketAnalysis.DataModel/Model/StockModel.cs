using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis
{
    public class StockModel
    {
        [JsonPropertyName("symbol")]
        public string symbol { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("price")]
        public float price { get; set; }

        [JsonPropertyName("exchange")]
        public string exchange { get; set; }
    }
}
