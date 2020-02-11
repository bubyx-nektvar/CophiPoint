using CophiPoint.Api;
using CophiPoint.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services.Implementation
{
    public class HtmlManager
    {
        private readonly IHttpRestService _httpService;

        public HtmlManager(IHttpRestService httpService)
        {
            _httpService = httpService;
        }

        public async Task<HtmlPage> GetPageAsync(string title, Func<Urls.InfoUrls, string> url)
        {
            var content = await _httpService.GetHtmlAsync(u => url(u.Info));

            return new HtmlPage(content, title);
        }
    }
}
