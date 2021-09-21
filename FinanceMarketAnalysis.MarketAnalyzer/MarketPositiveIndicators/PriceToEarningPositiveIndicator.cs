namespace FinanceMarketAnalysis
{
    internal class PriceToEarningPositiveIndicator : IMarketPositiveIndicator
    {
        public PriceToEarningPositiveIndicator(double priceToEarningRatio)
        {
            PriceToEarningRatio = priceToEarningRatio;
        }
        private const double PriceToEarningNormalRatio = 15;

        private double PriceToEarningRatio { get; set; }

        public bool IsPositive()
        {
            return PriceToEarningNormalRatio >= PriceToEarningRatio;
        }
    }
}
