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
        private HttpClient _sharedClient;
        private Urls _result;

        public ApiConnectionService()
        {
        }

        public async Task<Urls> GetUrls()
        {
            if (_result != null)
                return _result;

            var client = CreateHttpClient(x=>x);
            var response = await client.GetAsync(Config.ApiConfigUrl);
            _result = await response.ParseResultOrFail<Urls>();

            return _result;
        }

        private HttpClient CreateHttpClient(Func<HttpMessageHandler, HttpMessageHandler> addMiddleware)
        {
            var cacheService = TinyIoC.TinyIoCContainer.Current.Resolve<ICacheService>();

            var handler = DependencyService.Get<INativeHttpService>().GetNativeHandler();
            var middleware = addMiddleware(handler);
            return new HttpClient(new UrlCacheHandler(middleware, cacheService));
        }
        
        private async Task<HttpClient> GetHttpClient()
        {
            if (_sharedClient != null)
                return _sharedClient;

            var authService = TinyIoC.TinyIoCContainer.Current.Resolve<AuthService>();
            _sharedClient = CreateHttpClient(handler => new AuthorizationHandler(handler, authService));

            _sharedClient.BaseAddress = (await GetUrls()).GetBaseAddress();
            
            return _sharedClient;
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod method, Func<Urls, string> relativePathSelector, string mediaType, bool cache, HttpContent content = null, bool authorize = false)
        {
            var client = await GetHttpClient();
            var urls = await GetUrls();

            var url = relativePathSelector(urls);

            using (var message = new HttpRequestMessage(method, url))
            {
                message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

                if (authorize) {
                    AuthorizationHandler.SetDoAuthrozation(message);
                }
                
                if(content != null)
                {
                    message.Content = content;
                }

                message.Headers.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = !cache
                };

                return await client.SendAsync(message);
            }
        }

        public async Task<HttpResponseMessage> PostAuthorizedAsync<T>(T contentObject, Func<Urls, string> relativePathSelector)
        {
            var json = JsonConvert.SerializeObject(contentObject);
            var content = new StringContent(json, Encoding.UTF8, HttpExtension.JsonMediaType);

            return await SendAsync(HttpMethod.Post, relativePathSelector, HttpExtension.JsonMediaType, cache: false, content, authorize: true);
        }

        public async Task<TResponse> GetAuthorizedAsync<TResponse>(Func<Urls, string> relativePathSelector, bool cache = true)
        {
            var response = await SendAsync(HttpMethod.Get, relativePathSelector, HttpExtension.JsonMediaType, cache, authorize: true);
            return await response.ParseResultOrFail<TResponse>();
        }

        public async Task<TResponse> GetJsonAsync<TResponse>(Func<Urls,string> relativePathSelector)
        {
            var response = await SendAsync(HttpMethod.Get, relativePathSelector, HttpExtension.JsonMediaType, cache: true);
            return await response.ParseResultOrFail<TResponse>();
        }

        public async Task<string> GetHtmlAsync(Func<Urls, string> relativePathSelector)
        {
            using (var response = await SendAsync(HttpMethod.Get, relativePathSelector, HttpExtension.HtmlMediaType, cache: true))
            {
                response.EnsureSuccessStatusCode();
                if (response.Content.Headers.ContentType.MediaType != HttpExtension.HtmlMediaType)
                    throw new HttpRequestException(GeneralResources.MediaTypeError);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
