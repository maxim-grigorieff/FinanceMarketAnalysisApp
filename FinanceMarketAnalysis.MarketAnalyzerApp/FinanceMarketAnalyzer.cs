using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis.MarketAnalyzerApp
{
    internal class FinanceMarketAnalyzer
    {
        public FinanceMarketAnalyzer()
        {
            Logger = new Logger();
            FinanceDataStore = new FinanceDataStore(Logger);
        }

        private readonly ILogger Logger;
        private readonly IFinanceDataStore FinanceDataStore;

        public async Task<IEnumerable<StockDataDescriptor>> AnalyzeAsync()
        {
            var positiveStocks = new List<StockDataDescriptor>();
            await foreach (var stockDataDescriptor in FinanceDataStore.ReadAsync())
            {
                Logger.TraceInformation($"Start analyze stock: {stockDataDescriptor.symbol}.");
                if (ValidateMarketPositiveIndicator(stockDataDescriptor))
                {
                    positiveStocks.Add(stockDataDescriptor);
                }
            }
            return positiveStocks;
        }

        private bool ValidateMarketPositiveIndicator(StockDataDescriptor stockDataDescriptor)
        {
            var marketPositiveIndicator = MarketPositiveIndicatorFactory.Create(stockDataDescriptor);
            return marketPositiveIndicator.IsPositive();
        }
    }
}
