using System;

namespace FinanceMarketAnalysis
{
    public class FinanceDataReaderException : Exception
    {
        public FinanceDataReaderException(Exception exc)
            :base($"Unable to read the finance data. Error: {exc.Message}", exc)
        {
        }
    }
}
