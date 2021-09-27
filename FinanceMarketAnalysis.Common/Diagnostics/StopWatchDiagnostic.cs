using System;
using System.Diagnostics;

namespace FinanceMarketAnalysis
{
    public class StopWatchDiagnostic : IDisposable
    {
        public StopWatchDiagnostic()
        {
            StopWatch.Start();
        }
        private readonly Stopwatch StopWatch = new();

        public void Dispose()
        {
            StopWatch.Stop();
            Trace.TraceInformation($"Total elapsed time: {StopWatch.ElapsedMilliseconds}");
        }
    }
}
