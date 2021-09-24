using System;
using System.IO;
using System.Reflection;

namespace FinanceMarketAnalysis
{
    internal interface IEnvironmentVariableReader
    {
        string StockDataRepositoryPath { get; }
    }

    internal class EnvironmentVariableReader : IEnvironmentVariableReader
    {
        private const string StockDataRepoEnvName = "STOCK_DATA_REPOSITORY";

        private const string DirectoryName = @"FinanceMarketAnalysisApp\StockDataRepository";
        //private string DirectoryPath => $"{Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DirectoryName)}";
        private string DirectoryPath => $"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DirectoryName)}";

        public string StockDataRepositoryPath
        {
            get
            {
                var envStockRepo = Environment.GetEnvironmentVariable(StockDataRepoEnvName);
                return string.IsNullOrEmpty(envStockRepo) ? DirectoryPath : envStockRepo;
            }

        }
    }
}
