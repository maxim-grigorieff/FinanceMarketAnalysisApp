using System.Text.Json.Serialization;

namespace FinanceMarketAnalysis
{

    public class CompanyKeyMetricsModel
    {
        [JsonPropertyName("date")]
        public string date { get; set; }

        [JsonPropertyName("marketCap")]
        public double? marketCap { get; set; }

        [JsonPropertyName("peRatio")]
        public double? peRatio { get; set; }

        [JsonPropertyName("pbRatio")]
        public double? pbRatio { get; set; }

        [JsonPropertyName("currentRatio")]
        public double? currentRatio { get; set; }
    }

}
