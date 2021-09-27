using FinanceMarketAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceMarketAnalysisApp
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

        public async Task Analyze()
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

        private bool ValidateMarketPositiveIndicator(string symbol)
        {
            var inputData = new StockDataDescriptor
            {
                symbol = symbol,
                CompanyKeyMetrics   = DataReader.ReadCompanyKeyMetricsAsync(symbol).Result,
                FinanceStatements   = DataReader.ReadFinanceStatementsAsync(symbol).Result,
                HistoricalDividends = DataReader.ReadHistoricalDividendsAsync(symbol).Result
            };

            var marketPositiveIndicator = MarketPositiveIndicatorFactory.Create(inputData);
            return marketPositiveIndicator.IsPositive();
        }

        private void ReadCompanyKeyMetricsAsync(string symbol)
        {
            Trace.TraceInformation($"Read company key metrics for stock {symbol} data.");

            try
            {
                var result = DataReader.ReadCompanyKeyMetricsAsync(symbol).Result;
                //if (!result.Property1.Any())
                //{
                //    return;
                //}
                //var pe = ReadPriceToEarningsRatio(result.Property1.Last());
            }
            catch (Exception exc)
            {
                Trace.TraceError($"{exc.Message}");
            }
        }

        //private float ReadPriceToEarningsRatio(Class1 model)
        //{
        //    return model.peRatio;
        //}

        private void ReadHistoricalDividends(StockModel stock)
        {
            Trace.TraceInformation($"Read historical dividends for stock {stock.symbol} data.");

            try
            {
                var result = DataReader.ReadHistoricalDividendsAsync(stock.symbol).Result;
                if (result.historical == null)
                {
                    return;
                }
                var averageByYears = Read(result).ToList();
            }
            catch (Exception exc)
            {
                Trace.TraceError($"{exc.Message}");
            }
        }

        private IEnumerable<double> Read(HistoricalDividendsRootobjectModel stockModel)
        {
            const int lastYears = 5;

            var annualDividends = stockModel.historical.GroupBy(historicalDividends => ConvertTo(historicalDividends.date).GetValueOrDefault().Year);
            return annualDividends.OrderByDescending(dividendsByYear => dividendsByYear.Key)
                                  .Where(historicalDividendsByYear => !DateTime.UtcNow.Year.Equals(historicalDividendsByYear))
                                  .Take(lastYears)
                                  .Select(ConverToArithmeticMean);
        }

        private DateTime? ConvertTo(string rawDate)
        {
            return DateTime.TryParse(rawDate, out var result) ? result : null;
        }

        private double ConverToArithmeticMean(IEnumerable<HistoricalDividendsModel> historicalDividends)
        {
            return historicalDividends.Average(historicalDividends => historicalDividends.dividend);
        }
    }
}
