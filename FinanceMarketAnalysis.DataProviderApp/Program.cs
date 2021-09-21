using System;
using System.Diagnostics;

namespace FinanceMarketAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start download the finance data...");
            try
            {
                using (new StopWatchDiagnostic())
                {
                    var reader = new FinanceMarketAnalysisReader();
                    var completion = reader.Read();
                    completion.Wait();
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError(exc.Message);
            }
        }
    }
}
