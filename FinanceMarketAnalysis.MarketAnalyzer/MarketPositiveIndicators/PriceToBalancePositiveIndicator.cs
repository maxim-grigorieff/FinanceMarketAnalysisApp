namespace FinanceMarketAnalysis
{
    internal class PriceToBalancePositiveIndicator : IMarketPositiveIndicator
    {
        public PriceToBalancePositiveIndicator(double priceToBalanceRatio)
        {
            PriceToBalanceRatio = priceToBalanceRatio;
        }
        private const double PriceToBalanceNormalRatio = 1.5;

        private double PriceToBalanceRatio { get; set; }

        public bool IsPositive()
        {
            return PriceToBalanceNormalRatio >= PriceToBalanceRatio;
        }
    }
}
