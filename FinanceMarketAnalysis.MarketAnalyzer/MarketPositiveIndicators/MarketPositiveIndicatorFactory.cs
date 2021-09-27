using FinanceMarketAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceMarketAnalysis
{
    public static class MarketPositiveIndicatorFactory
    {
        private static double ConvertTo(IEnumerable<CompanyKeyMetricsModel> companyKeyMetrics, Func<CompanyKeyMetricsModel, double> funcSelector)
        {
            return companyKeyMetrics.OrderByDescending(companyKeyMetrics => DateTime.Parse(companyKeyMetrics.date))
                        .Select(funcSelector)
                        .First();
        }

        private static IEnumerable<FinanceStatementModel> ConvertTo(IEnumerable<FinanceStatementModel> financeStatements)
        {
            return financeStatements.OrderByDescending(financeStatement => DateTime.Parse(financeStatement.date));
        }

        public static bool Validate(StockDataDescriptor inputData)
        {
            return inputData.CompanyKeyMetrics.Any() && 
                   inputData.FinanceStatements.Any() && 
                   (inputData.HistoricalDividends.historical?.Any() ?? false);
        }

        public static IMarketPositiveIndicator Create(StockDataDescriptor inputData)
        {
            return new MarketPositiveIndicators(new IMarketPositiveIndicator[] 
            {
                new MarketCapPositiveIndicator(ConvertTo(inputData.CompanyKeyMetrics, (keyMetrics) => keyMetrics.marketCap.GetValueOrDefault())),
                new LiquidityPositiveIndicator(ConvertTo(inputData.CompanyKeyMetrics, (keyMetrics) => keyMetrics.currentRatio.GetValueOrDefault())),
                new PriceToEarningPositiveIndicator(ConvertTo(inputData.CompanyKeyMetrics, (keyMetrics) => keyMetrics.peRatio.GetValueOrDefault())),
                new PriceToBalancePositiveIndicator(ConvertTo(inputData.CompanyKeyMetrics, (keyMetrics) => keyMetrics.pbRatio.GetValueOrDefault())),
                new LongTermNetIncomePositiveIndicator(ConvertTo(inputData.FinanceStatements).Select(financeStatement => financeStatement.netIncome)),
                new LongTermEarningsPerSharePositiveIndicator(ConvertTo(inputData.FinanceStatements)),
                new LongTermHistoricalDividendsPositiveIndicator(inputData.HistoricalDividends),
            });
        }
    }
}
