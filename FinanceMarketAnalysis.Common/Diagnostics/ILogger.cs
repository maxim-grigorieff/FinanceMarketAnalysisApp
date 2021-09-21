namespace FinanceMarketAnalysis
{
    public interface ILogger
    {
        void TraceInformation(string message);
        void TraceError(string message);
    }
}
