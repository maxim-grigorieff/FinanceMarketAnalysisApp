using System.Text.Json.Serialization;

namespace FinanceMarketAnalysis
{
    public class FinanceStatementModel
    {
        [JsonPropertyName("date")]
        public string date { get; set; }

        [JsonPropertyName("eps")]
        public double eps { get; set; }

        [JsonPropertyName("netIncome")]
        public double netIncome { get; set; }
        
    }

}
