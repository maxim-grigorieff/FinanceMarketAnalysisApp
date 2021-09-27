using AngleSharp.Html.Parser;
using FinanceMarketAnalysis.DataProvider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis
{
    public class Sp500ListDataReader : ISp500ListDataReader
    {

        public Sp500ListDataReader(ILogger logger)
        {
            Logger = logger;
        }

        private const string RequestUri = "https://en.wikipedia.org/wiki/List_of_S%26P_500_companies";
        private readonly Lazy<HttpClient> HttpClientProxy = new(new HttpClient());
        private ILogger Logger { get; }

        public async Task<IEnumerable<string>> ReadSp500StocksAsync()
        {
            Logger.TraceInformation($"Start request {RequestUri}.");

            using var request = new HttpRequestMessage(new HttpMethod("GET"), RequestUri);
            var response = await HttpClientProxy.Value.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var spList = await ParseHtml(responseStream);
            if (!spList.Any())
            {
                throw new FinanceDataReaderException(new ArgumentException(DataProviderResource.UnableParseSP500Message));
            }
            return spList;
        }

        private async Task<IEnumerable<string>> ParseHtml(Stream stream)
        {
            var parser = new HtmlParser();

            using var doc = await parser.ParseDocumentAsync(stream);
            var spList = doc.QuerySelectorAll("#constituents  tr")
                            .Skip(1)
                            .Select(row => row.Children[0].QuerySelector(".external.text"));

            return spList.Select(symbol => symbol.TextContent);
        }
    }
}
