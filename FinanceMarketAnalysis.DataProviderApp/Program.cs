using System;
using System.Diagnostics;
using System.Linq;

namespace FinanceMarketAnalysis
{
    class Program
    {

        static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            Trace.TraceInformation("Start download the finance data...");
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
                return -1;
            }
            return 0;
        }
    }
}
