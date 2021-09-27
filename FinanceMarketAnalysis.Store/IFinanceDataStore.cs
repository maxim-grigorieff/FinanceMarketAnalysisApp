using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis
{
    public interface IFinanceDataStore
    {
        Task WriteAsync(StockDataDescriptor stockDataDescriptor);
        IAsyncEnumerable<StockDataDescriptor> ReadAsync();
    }
}
