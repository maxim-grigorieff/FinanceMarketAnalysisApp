namespace FinanceMarketAnalysis
{
    internal class LiquidityPositiveIndicator : IMarketPositiveIndicator
    {
        public LiquidityPositiveIndicator(double currentRatio)
        {
            CurrentRatio = currentRatio;
        }
        private const double NormalCurrentRatio = 2;

        private double CurrentRatio { get; set; }

        public bool IsPositive()
        {
            return CurrentRatio >= NormalCurrentRatio;
        }
    }
}
