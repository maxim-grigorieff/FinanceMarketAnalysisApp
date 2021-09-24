using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis
{
    public class FinanceDataStore : IFinanceDataStore
    {
        public FinanceDataStore(ILogger logger)
        {
            Logger = logger;
            EnvVariableReader = new EnvironmentVariableReader();
        }

        private const string DirectoryName = "StockDataRepository";
        private const string FileExtension = ".json";

        private string DirectoryPath => $"{EnvVariableReader.StockDataRepositoryPath}";
        private ILogger Logger { get; }
        private IEnvironmentVariableReader EnvVariableReader { get; }

        private string ConvertToFileName(string symbol)
        {
            return $"{symbol}_{DateTime.UtcNow:MM.dd.yyyy_HH.mm.ss}{FileExtension}";
        }

        public async Task WriteAsync(StockDataDescriptor stockDataDescriptor)
        {
            var directoryInfo = CreateDirectoryIfNotExists();
            var filePath = Path.Combine(directoryInfo.FullName, ConvertToFileName(stockDataDescriptor.symbol));

            Logger.TraceInformation($"Start write to file: {filePath}.");
            using var stream = new FileStream(filePath, FileMode.Create);
            
            await JsonSerializer.SerializeAsync(stream, stockDataDescriptor);
            Logger.TraceInformation($"Write to file: {filePath} completed.");
        }

        private DirectoryInfo CreateDirectoryIfNotExists()
        {
            var directoryInfo = new DirectoryInfo(DirectoryPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            return directoryInfo;
        }

        public async IAsyncEnumerable<StockDataDescriptor> ReadAsync()
        {
            var directoryInfo = CreateDirectoryIfNotExists();
            foreach(var file in directoryInfo.EnumerateFiles($"*{FileExtension}"))
            {
                yield return await ReadAsync(file.FullName);
            }
        }

        public async Task<StockDataDescriptor> ReadAsync(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<StockDataDescriptor>(stream);
        }
    }
}
