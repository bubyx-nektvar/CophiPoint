using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Api
{
    public class ApiConnectionService
    {
        private readonly HttpClient _client;
        private HttpClient _sharedClient;
        private Urls _result;

        public ApiConnectionService()
        {
            _client = new HttpClient();
        }

        public async Task<Urls> GetUrls()
        {
            if (_result != null)
                return _result;

            var response = await _client.GetAsync(Config.ApiConfigUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _result = JsonConvert.DeserializeObject<Urls>(content);
            }
            return _result;
        }
        
        private async Task<HttpClient> GetHttpClient()
        {
            if (_sharedClient != null)
                return _sharedClient;
            
            _sharedClient = new HttpClient();
            _sharedClient.BaseAddress = (await GetUrls()).GetBaseAddress();
            _sharedClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            return _sharedClient;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, Func<Urls, string> relativePathSelector, Action<HttpRequestMessage> prepareMessage = null)
        {
            var client = await GetHttpClient();
            var urls = await GetUrls();

            var url = relativePathSelector(urls);

            using (var message = new HttpRequestMessage(method, url))
            {
                prepareMessage?.Invoke(message);

                return await client.SendAsync(message);
            }
        }
    }
}
