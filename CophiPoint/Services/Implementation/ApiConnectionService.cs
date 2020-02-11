using CophiPoint.Api;
using CophiPoint.Extensions;
using CophiPoint.Helpers.HttpHandlers;
using CophiPoint.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint.Services.Implementation
{
    public class ApiConnectionService: IHttpRestService
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

            var service = TinyIoC.TinyIoCContainer.Current.Resolve<AuthService>();
            var handler = DependencyService.Get<INativeHttpService>().GetNativeHandler();
            
            _sharedClient = new HttpClient(new AuthorizationHandler(handler, service));
            _sharedClient.BaseAddress = (await GetUrls()).GetBaseAddress();
            _sharedClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            return _sharedClient;
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod method, Func<Urls, string> relativePathSelector, HttpContent content = null, bool authorize = false)
        {
            var client = await GetHttpClient();
            var urls = await GetUrls();

            var url = relativePathSelector(urls);

            using (var message = new HttpRequestMessage(method, url))
            {
                if (authorize) {
                    AuthorizationHandler.SetDoAuthrozation(message);
                }
                
                if(content != null)
                {
                    message.Content = content;
                }

                return await client.SendAsync(message);
            }
        }

        public async Task<HttpResponseMessage> PostAuthorizedAsync<T>(T contentObject, Func<Urls, string> relativePathSelector)
        {
            var json = JsonConvert.SerializeObject(contentObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await SendAsync(HttpMethod.Post, relativePathSelector, content, true);
        }

        public async Task<TResponse> GetAuthorizedAsync<TResponse>(Func<Urls, string> relativePathSelector)
        {
            var response = await SendAsync(HttpMethod.Get, relativePathSelector, authorize: true);
            return await response.ParseResultOrFail<TResponse>();
        }

        public async Task<TResponse> GetAsync<TResponse>(Func<Urls,string> relativePathSelector)
        {
            var response = await SendAsync(HttpMethod.Get, relativePathSelector);
            return await response.ParseResultOrFail<TResponse>();
        }
    }
}
