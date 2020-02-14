using CophiPoint.Helpers.HttpHandlers.HttpExceptions;
using CophiPoint.Services;
using CophiPoint.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CophiPoint.Helpers.HttpHandlers
{
    public class AuthorizationHandler: DelegatingHandler
    {
        private readonly AuthService _authService;
        private const string AuthorizeProperty = "performAuth";

        public AuthorizationHandler(HttpMessageHandler innerHandler, AuthService authService) :base(innerHandler)
        {
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Properties.ContainsKey(AuthorizeProperty))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            request.Headers.Authorization = await _authService.GetAccessToken();
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch(HttpStatusCodeException ex) when (ex.Status == System.Net.HttpStatusCode.Unauthorized)
            {
                request.Headers.Authorization = await _authService.GetAccessToken();
                return await base.SendAsync(request, cancellationToken);
            }
        }
        
        public static void SetDoAuthrozation(HttpRequestMessage request)
        {
            request.Properties.Add(AuthorizeProperty, true);
        }
    }
}
