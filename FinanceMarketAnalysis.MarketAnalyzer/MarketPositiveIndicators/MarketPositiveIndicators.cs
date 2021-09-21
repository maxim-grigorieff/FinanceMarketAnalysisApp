using System.Collections.Generic;
using System.Linq;

namespace FinanceMarketAnalysis
{
    internal class MarketPositiveIndicators : IMarketPositiveIndicator
    {
        public MarketPositiveIndicators(IEnumerable<IMarketPositiveIndicator> positiveIndicators)
        {
            PositiveIndicators = positiveIndicators;
        }
        private IEnumerable<IMarketPositiveIndicator> PositiveIndicators { get; }

        public bool IsPositive()
        {
            return PositiveIndicators.All(positiveIndicator => positiveIndicator.IsPositive());
        }
    }
}
