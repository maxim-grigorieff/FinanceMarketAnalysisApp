using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis
{
    public interface ISp500ListDataReader
    {
        Task<IEnumerable<string>> ReadSp500StocksAsync();
    }
}
