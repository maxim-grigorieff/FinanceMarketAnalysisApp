using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FinanceMarketAnalysis.Tests")]

namespace FinanceMarketAnalysis
{
    public interface IMarketPositiveIndicator
    {
        bool IsPositive();
    }
}
