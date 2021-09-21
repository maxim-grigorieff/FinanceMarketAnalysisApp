using FinanceMarketAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceMarketAnalysis
{
    internal class LongTermHistoricalDividendsPositiveIndicator : IMarketPositiveIndicator
    {
        public LongTermHistoricalDividendsPositiveIndicator(HistoricalDividendsRootobjectModel historicalDividends)
        {
            HistoricalDividends = historicalDividends;
        }

        private const int lastTenYears = 10;
        private readonly static IEnumerable<int> lastYears = Enumerable.Range(DateTime.UtcNow.Year - 10, 10);

        private HistoricalDividendsRootobjectModel HistoricalDividends { get; set; }

        public bool IsPositive()
        {
            if (HistoricalDividends.historical == null)
            {
                return false;
            }

            var annualDividends    = HistoricalDividends.historical.GroupBy(historicalDividends => ConvertTo(historicalDividends.date).GetValueOrDefault().Year);
            var lastYearsDividends = annualDividends.OrderByDescending(dividendsByYear => dividendsByYear.Key)
                                                    .Where(dividendsByYear => !DateTime.UtcNow.Year.Equals(dividendsByYear.Key))
                                                    .Take(lastTenYears).ToList();

            return lastYears.All((year) => lastYearsDividends.Any(yearDividends => HasPositiveDividends(year, yearDividends) ));
        }

        private bool HasPositiveDividends(int year, IGrouping<int, HistoricalDividendsModel> yearDividends)
        {
            return yearDividends.Key.Equals(year) && 
                   yearDividends.Select(historicalDiv => historicalDiv.dividend).Sum() > 0;
        }

        private DateTime? ConvertTo(string rawDate)
        {
            return DateTime.TryParse(rawDate, out var result) ? result : null;
        }
    }
}
