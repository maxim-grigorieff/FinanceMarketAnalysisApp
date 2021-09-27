using System;
using System.IO;
using System.Reflection;

namespace FinanceMarketAnalysis
{
    public interface IEnvironmentVariableReader
    {
        string ApiKey { get; }
        string StockDataRepositoryPath { get; }
    }

    public class EnvironmentVariableReader : IEnvironmentVariableReader
    {
        private const string StockDataRepoEnvName = "STOCK_DATA_REPOSITORY";
        private const string FmpApiKeyEnvName = "FMP_API_KEY";

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

        public string ApiKey => Environment.GetEnvironmentVariable(FmpApiKeyEnvName);
    }
}
