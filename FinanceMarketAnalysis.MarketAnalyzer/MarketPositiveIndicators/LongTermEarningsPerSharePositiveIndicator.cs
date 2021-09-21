using FinanceMarketAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceMarketAnalysis
{
    internal class LongTermEarningsPerSharePositiveIndicator : IMarketPositiveIndicator
    {
        public LongTermEarningsPerSharePositiveIndicator(IEnumerable<FinanceStatementModel> financeStatements)
        {
            FinanceStatements = financeStatements;
        }

        private const float NormalEarningsPerSharePercent = 50;

        private IEnumerable<FinanceStatementModel> FinanceStatements { get; set; }


        public bool IsPositive()
        {
            var earningsPerShare = FinanceStatements.Select(finStatement => (DateTime.Parse(finStatement.date), finStatement.eps))
                                                    .OrderBy(finStatement => finStatement.Item1)
                                                    .Select(finStatement => finStatement.eps).ToList();

            var middle = earningsPerShare.Count() / 2;
            var firstPeriod = earningsPerShare.Take(middle);
            var lastPeriod  = earningsPerShare.Skip(middle);

            var firstEpsAgerage = firstPeriod.Average();
            var lastEpsAgerage  = lastPeriod.Average();

            if (0 > firstEpsAgerage || 0 > lastEpsAgerage)
            {
                return false;
            }
            var percent = (lastEpsAgerage / firstEpsAgerage) * 100;

            return percent >= NormalEarningsPerSharePercent;
        }
    }
}
