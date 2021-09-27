using FinanceMarketAnalysis;
using System;
using System.Diagnostics;

namespace FinanceMarketAnalysisApp
{

    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                using (new StopWatchDiagnostic())
                {
                    var reader = new FinanceMarketAnalysisReader();
                    var completion = reader.Analyze();
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
