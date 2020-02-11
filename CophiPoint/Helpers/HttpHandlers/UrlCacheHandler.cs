using CophiPoint.Extensions;
using CophiPoint.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CophiPoint.Helpers.HttpHandlers
{
    public class UrlCacheHandler : DelegatingHandler
    {
        private const string VersionHeader = "X-Static-Data-Version";
        private readonly ICacheService _cache;

        public UrlCacheHandler(HttpMessageHandler innerHandler, ICacheService cache)
            : base(innerHandler)
        {
            _cache = cache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method != HttpMethod.Get
                || request.Headers.CacheControl.NoCache)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            if (_cache.Contains(request.RequestUri))
            {
                var cachedItem = _cache.Get(request.RequestUri);
                return CreateResponse(cachedItem, request, HttpStatusCode.OK);
                
            }
            else
            {
                var result = await base.SendAsync(request, cancellationToken);
                if (result.IsSuccessStatusCode)
                {
                    using (result)
                    {
                        var value = await result.Content.ReadAsStringAsync();
                        var version = result.Headers.GetValues(VersionHeader).First();

                        var cachedItem = new CachedItem()
                        {
                            Content = value,
                            Version = version,
                            MediaType = result.Content.Headers.ContentType.MediaType
                        };

                        await _cache.Set(request.RequestUri, cachedItem);

                        return CreateResponse(cachedItem, request, result.StatusCode);
                    }
                }
                else
                {
                    return result;
                }
            }
        }

        private HttpResponseMessage CreateResponse(CachedItem cachedItem,HttpRequestMessage request, HttpStatusCode status)
        {
            var response = new HttpResponseMessage(status)
            {
                RequestMessage = request,

                Content = new StringContent(cachedItem.Content, Encoding.UTF8, cachedItem.MediaType)
            };

            return response;
        }
    }
}
