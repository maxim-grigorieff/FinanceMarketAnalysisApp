using System;
using System.Diagnostics;
using System.Linq;

namespace FinanceMarketAnalysis
{
    class Program
    {
        internal class StocksDataReader
        {
            private const int TotalNumberOfAttempts = 1000;

            private int NumberOfAttempts = 0;

            public void Read()
            {
                try
                {
                    TryRead();
                }
                catch (Exception exc) when (HasFreeApiConstraint(exc))
                {
                    Trace.TraceError(exc.Message);
                    NumberOfAttempts++;
                    if (TotalNumberOfAttempts == NumberOfAttempts)
                    {
                        return;
                    }
                    Read();
                }
            }
            
            private static bool HasFreeApiConstraint(Exception exc)
            {
                if (exc.GetType() == typeof(FreeApiConstraintFinanceDataReaderException))
                {
                    return true;
                }
                if (exc.InnerException != null)
                {
                    return HasFreeApiConstraint(exc.InnerException);
                }
                return false;
            }

            private void TryRead()
            {
                var reader = new FinanceMarketAnalysisReader();
                var completion = reader.Read();
                completion.Wait();
            }
        }

        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            Trace.WriteLine("Start download the finance data...");
            try
            {
                using (new StopWatchDiagnostic())
                {
                    var stocksDataReader = new StocksDataReader();
                    stocksDataReader.Read();
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError(exc.Message);
            }
        }
    }
}
