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
            try
            {
                if (!request.Properties.ContainsKey(AuthorizeProperty))
                {
                    return await base.SendAsync(request, cancellationToken);
                }

                request.Headers.Authorization = await _authService.GetAccessToken();

                var response = await base.SendAsync(request, cancellationToken);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    request.Headers.Authorization = await _authService.GetAccessToken();
                    return await base.SendAsync(request, cancellationToken);
                }
                return response;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        
        public static void SetDoAuthrozation(HttpRequestMessage request)
        {
            request.Properties.Add(AuthorizeProperty, true);
        }
    }
}
