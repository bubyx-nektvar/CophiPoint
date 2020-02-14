using CophiPoint.Helpers.HttpHandlers.HttpExceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CophiPoint.Helpers.HttpHandlers
{
    public class HttpExceptionThrowHandler : DelegatingHandler
    {
        public HttpExceptionThrowHandler(HttpMessageHandler innerHandler)
            :base(innerHandler)
        { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = await base.SendAsync(request, cancellationToken);
            if (!result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                throw new HttpStatusCodeException(result.StatusCode, content);
            }

            return result;
        }
    }
}
