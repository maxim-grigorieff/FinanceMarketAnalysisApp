using System;

namespace FinanceMarketAnalysis
{
    public class FreeApiConstraintFinanceDataReaderException : FinanceDataReaderException
    {
        public FreeApiConstraintFinanceDataReaderException(Exception exc)
            :base(exc)
        {
        }
    }
}
