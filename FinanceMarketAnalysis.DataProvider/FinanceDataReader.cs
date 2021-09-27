using AngleSharp;
using FinanceMarketAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis
{

    public class FinanceDataReader : IFinanceDataApiReader
    {
        public FinanceDataReader(ILogger logger, string apiKey)
        {
            Logger = logger;
            ApiKey = apiKey;
        }

        private const string BaseUri = "https://financialmodelingprep.com/api/v3/";

        private readonly Lazy<HttpClient> HttpClientProxy = new(new HttpClient());
        private ILogger Logger { get; }
        private string  ApiKey { get; }

        private string GetStockListUri()
        {
            return $"{BaseUri}stock/list?apikey={ApiKey}";
        }

        private string GetSP500ListUri()
        {
            return $"{BaseUri}sp500_constituent?apikey={ApiKey}";
        }

        private string GetTradableStockListUri()
        {
            return $"{BaseUri}available-traded/list?apikey={ApiKey}";
        }

        private string GetHistoricalDividendsUri(string symbol)
        {
            return $"{BaseUri}historical-price-full/stock_dividend/{symbol}?apikey={ApiKey}";
        }

        private string GetCompanyKeyMetrics(string symbol)
        {
            return $"{BaseUri}key-metrics/{symbol}?apikey={ApiKey}";
        }

        private string GetFinanceStatements(string symbol)
        {
            return $"{BaseUri}income-statement/{symbol}?apikey={ApiKey}";
        }

        private string GetStockBatchRequest(IEnumerable<string> symbols)
        {
            return $"{BaseUri}quote/{string.Join(",", symbols)}?apikey={ApiKey}";
        }

        public async Task<IEnumerable<StockModel>> ReadStocksAsync()
        {
            return await InternalReadAsync<IEnumerable<StockModel>>(GetStockListUri());
        }

        public async Task<IEnumerable<StockModel>> ReadTradableStocksAsync()
        {
            return await InternalReadAsync<IEnumerable<StockModel>>(GetTradableStockListUri());
        }

        public async Task<IEnumerable<Sp500StockModel>> ReadSp500StocksAsync()
        {
            return await InternalReadAsync<IEnumerable<Sp500StockModel>>(GetSP500ListUri());
        }

        private async Task<ModelType> InternalReadAsync<ModelType>(string requestUri)
        {
            Logger.TraceInformation($"Start request {requestUri}.");

            using var request = new HttpRequestMessage(new HttpMethod("GET"), requestUri);
            request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            var response = await HttpClientProxy.Value.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var res = await response.Content.ReadAsStringAsync();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            Logger.TraceInformation($"Request {requestUri} completed.");

            return await ReadModel<ModelType>(responseStream);
        }

        private async Task<ModelType> ReadModel<ModelType>(Stream stream)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<ModelType>(stream, new JsonSerializerOptions { IgnoreNullValues = true });
            }
            catch (Exception exc)
            {
                var result = await TryParseFreeApiConstraints(stream);
                if (result)
                {
                    throw new FreeApiConstraintFinanceDataReaderException(exc);
                }
                throw;
            }
        }

        private async Task<bool> TryParseFreeApiConstraints(Stream stream)
        {
            try
            {
                stream.Position = 0;
                var response = await JsonSerializer.DeserializeAsync<FreeApiConstraintModel>(stream, new JsonSerializerOptions { IgnoreNullValues = true });
                return true;    
            }
            catch (Exception exc)
            {
                Trace.TraceError($"Unable to parse json. Error: {exc.Message}");
                return false;
            }

        }

        public async Task<HistoricalDividendsRootobjectModel> ReadHistoricalDividendsAsync(string symbol)
        {
            return await InternalReadAsync<HistoricalDividendsRootobjectModel>(GetHistoricalDividendsUri(symbol));
            // using var request = new HttpRequestMessage(new HttpMethod("GET"), GetHistoricalDividendsUri(symbol));
            // request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            // var response = await HttpClientProxy.Value.SendAsync(request);
            // response.EnsureSuccessStatusCode();

            //return await response.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<CompanyKeyMetricsModel>> ReadCompanyKeyMetricsAsync(string symbol)
        {
            return await InternalReadAsync<IEnumerable<CompanyKeyMetricsModel>>(GetCompanyKeyMetrics(symbol));
        }

        public async Task<IEnumerable<FinanceStatementModel>> ReadFinanceStatementsAsync(string symbol)
        {
            return await InternalReadAsync<IEnumerable<FinanceStatementModel>>(GetFinanceStatements(symbol));
        }

        public async Task<IEnumerable<StockBatchRequestModel>> ReadStockBatch(IEnumerable<string> symbols)
        {
            return await InternalReadAsync<IEnumerable<StockBatchRequestModel>>(GetStockBatchRequest(symbols));
        }

        public void Dispose()
        {
            if (HttpClientProxy.IsValueCreated)
            {
                HttpClientProxy.Value.Dispose();
            }
        }
    }
}
