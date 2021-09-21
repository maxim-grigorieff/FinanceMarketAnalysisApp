using FinanceMarketAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis
{
    internal class FinanceMarketAnalysisReader
    {
        public FinanceMarketAnalysisReader()
        {
            Logger = new Logger();
            DataReader = new FinanceDataReader(Logger);
            Sp500DataReader = new Sp500ListDataReader(Logger);
            FinanceDataStore = new FinanceDataStore(Logger);
        }

        private readonly ILogger Logger;
        private readonly IFinanceDataApiReader DataReader;
        private readonly ISp500ListDataReader Sp500DataReader;
        private readonly IFinanceDataStore    FinanceDataStore;

        public async Task Read()
        {
            var writeAsync = new List<Task>();
            
            var sp500List = ReadSp500Stocks();
            if (!sp500List.Any())
            {
                Logger.TraceInformation($"Stock cache is completed.");
                return;
            }
            foreach (var symbol in sp500List)
            {
                var stockAsyncTask = ReadStockDataDescriptorAsync(symbol);
                var writeAsyncTask = FinanceDataStore.WriteAsync(stockAsyncTask.Result);
                writeAsync.Add(writeAsyncTask);
                //await Task.Delay(TimeSpan.FromSeconds(2));
            }
            await Task.WhenAll(writeAsync);
        }

        private IEnumerable<string> ReadSp500Stocks()
        {
            var storeStocks = ReadStockCacheAsync().Result;
            var sp500List   = Sp500DataReader.ReadSp500StocksAsync().Result;

            return sp500List.Except(storeStocks).Take(20);
        }

        private async Task<IEnumerable<string>> ReadStockCacheAsync()
        {
            var storeStocks = new List<StockDataDescriptor>();
            await foreach (var stockDataDescriptor in FinanceDataStore.ReadAsync())
            {
                storeStocks.Add(stockDataDescriptor);
            }
            return storeStocks.Select(stock => stock.symbol);
        }

        private async Task<StockDataDescriptor> ReadStockDataDescriptorAsync(string symbol)
        {
            Logger.TraceInformation($"Start read stock {symbol}.");

            var companyKeyMetrics   = DataReader.ReadCompanyKeyMetricsAsync(symbol);
            var financeStatements   = DataReader.ReadFinanceStatementsAsync(symbol);
            var historicalDividends = DataReader.ReadHistoricalDividendsAsync(symbol);

            await Task.WhenAll(new Task[] { companyKeyMetrics, financeStatements, historicalDividends });

            Logger.TraceInformation($"Read stock {symbol} data descriptor completed.");
            return new StockDataDescriptor
            {
                symbol = symbol,
                CompanyKeyMetrics   = companyKeyMetrics.Result,
                FinanceStatements   = financeStatements.Result,
                HistoricalDividends = historicalDividends.Result
            };
        }
    }
}
