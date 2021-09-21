using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FinanceMarketAnalysis.MarketAnalyzerApp
{
    class Program
    {
        private static void WriteToConsole(IEnumerable<StockDataDescriptor> stocks)
        {
            foreach(var stock in stocks)
            {
                Console.WriteLine($"The stock {stock.symbol} is suitable B. Graham metrics.");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Start analyze market stocks...");
            try
            {
                using (new StopWatchDiagnostic())
                {
                    var reader = new FinanceMarketAnalyzer();
                    var completion = reader.AnalyzeAsync();
                    WriteToConsole(completion.Result);
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError(exc.Message);
            }
        }
    }
}
