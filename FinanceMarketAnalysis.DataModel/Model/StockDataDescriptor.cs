using FinanceMarketAnalysis;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FinanceMarketAnalysis
{
    public class StockDataDescriptor
    {
        [JsonPropertyName("symbol")]
        public string symbol { get; set; }

        [JsonPropertyName("CompanyKeyMetrics")]
        public IEnumerable<CompanyKeyMetricsModel> CompanyKeyMetrics   { get; set; }
        
        [JsonPropertyName("FinanceStatements")]
        public IEnumerable<FinanceStatementModel>  FinanceStatements   { get; set; }

        [JsonPropertyName("HistoricalDividends")]
        public HistoricalDividendsRootobjectModel  HistoricalDividends { get; set; }
    }
}
