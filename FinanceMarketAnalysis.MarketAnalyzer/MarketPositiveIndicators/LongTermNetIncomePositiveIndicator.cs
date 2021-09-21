using System.Collections.Generic;
using System.Linq;

namespace FinanceMarketAnalysis
{
    internal class LongTermNetIncomePositiveIndicator : IMarketPositiveIndicator
    {
        public LongTermNetIncomePositiveIndicator(IEnumerable<double> netIncomes)
        {
            NetIncomes = netIncomes;
        }
        private IEnumerable<double> NetIncomes { get; set; }
        private const int LastFiveYears = 5;

        public bool IsPositive()
        {
            return NetIncomes.Take(LastFiveYears).All(netIncome => netIncome > 0);
        }
    }
}
