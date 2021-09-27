namespace FinanceMarketAnalysis
{
    internal class MarketCapPositiveIndicator : IMarketPositiveIndicator
    {
        public MarketCapPositiveIndicator(double marketCap)
        {
            MarketCap = marketCap;
        }
        private const double NormalMarketCap = 2000000000;

        private double MarketCap { get; set; }

        public bool IsPositive()
        {
            return MarketCap >= NormalMarketCap;
        }
    }
}
