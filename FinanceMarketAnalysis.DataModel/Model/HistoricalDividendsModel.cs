using System.Text.Json.Serialization;

namespace FinanceMarketAnalysis
{
    public class HistoricalDividendsRootobjectModel
    {
        [JsonPropertyName("symbol")]
        public string symbol { get; set; }

        [JsonPropertyName("historical")]
        public HistoricalDividendsModel[] historical { get; set; }
    }

    public class HistoricalDividendsModel
    {
        [JsonPropertyName("date")]
        public string date { get; set; }

        [JsonPropertyName("label")]
        public string label { get; set; }

        [JsonPropertyName("adjDividend")]
        public double adjDividend { get; set; }

        [JsonPropertyName("dividend")]
        public double dividend { get; set; }

        [JsonPropertyName("recordDate")]
        public string recordDate { get; set; }

        [JsonPropertyName("paymentDate")]
        public string paymentDate { get; set; }

        [JsonPropertyName("declarationDate")]
        public string declarationDate { get; set; }
    }

}
