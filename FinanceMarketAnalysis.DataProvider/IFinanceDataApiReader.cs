using FinanceMarketAnalysis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis
{
    public interface IFinanceDataApiReader : IDisposable
    {
        Task<IEnumerable<StockModel>>               ReadStocksAsync();
        Task<IEnumerable<StockModel>>               ReadTradableStocksAsync();
        Task<IEnumerable<Sp500StockModel>>          ReadSp500StocksAsync();
        Task<HistoricalDividendsRootobjectModel>    ReadHistoricalDividendsAsync(string symbol);
        Task<IEnumerable<CompanyKeyMetricsModel>>   ReadCompanyKeyMetricsAsync(string symbol);
        Task<IEnumerable<FinanceStatementModel>>    ReadFinanceStatementsAsync(string symbol);
        Task<IEnumerable<StockBatchRequestModel>>   ReadStockBatch(IEnumerable<string> symbols);
    }
}
