using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinanceMarketAnalysis.MarketAnalyzerWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        private readonly Uri RequestUri = new("http://FinanceMarketAnalysis.MarketAnalyzerWebApi/WeatherForecast");
        private readonly HttpClient Client = new System.Net.Http.HttpClient();

        public async Task OnGet()
        {
            ViewData["Message"] += await RequestApi();
        }

        private async Task<string> RequestApi()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = RequestUri
            };
            var response = await Client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
