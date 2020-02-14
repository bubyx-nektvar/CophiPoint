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
                || (request.Headers.CacheControl?.NoCache ?? false))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            if (await _cache.IsUpToDate(request.RequestUri))
            {
                var cachedItem = await _cache.GetValue(request.RequestUri);
                return CreateResponse(cachedItem.content, cachedItem.mediaType, request, HttpStatusCode.OK);
                
            }
            else
            {
                using (var result = await base.SendAsync(request, cancellationToken))
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var version = result.Headers.GetValues(VersionHeader).First();
                    var mediaType = result.Content.Headers.ContentType.MediaType;

                    await _cache.SetValue(request.RequestUri, version, content, mediaType);

                    return CreateResponse(content, mediaType, request, result.StatusCode);
                }
            }
        }

        private HttpResponseMessage CreateResponse(string content, string mediaType, HttpRequestMessage request, HttpStatusCode status)
        {
            var response = new HttpResponseMessage(status)
            {
                RequestMessage = request,

                Content = new StringContent(content, Encoding.UTF8, mediaType)
            };

            return response;
        }
    }
}
