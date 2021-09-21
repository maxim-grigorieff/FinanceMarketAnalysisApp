using System;
using System.Diagnostics;

namespace FinanceMarketAnalysis
{
    public class Logger : ILogger
    {
        private string DatePrefix => $"{DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss.fffK")}. ";
        
        public void TraceInformation(string message)
        {
            Trace.TraceInformation($"DEBUGGER: {DatePrefix}{message}");
        }

        public void TraceError(string message)
        {
            Trace.TraceError($"{DatePrefix}{message}");
        }
    }
}
