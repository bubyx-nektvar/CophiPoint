using CophiPoint.Api;
using CophiPoint.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint.Services.Implementation
{
    public class AuthService
    {
        private readonly IHttpRestService _connectionService;

        public AuthService(IHttpRestService initService)
        {
            this._connectionService = initService;
        }

        public bool IsLoggedIn
        {
            get
            {
                return DependencyService.Get<INativeAuthService>().IsLogged;
            }
        }

        public async Task<(string accessToken, string idToken)> GetToken()
        {
            try
            {
                return await DependencyService.Get<INativeAuthService>().GetTokens();
            }
            catch (UnauthorizedAccessException ex)
            {
                var result = await Login();
                if (result.IsSucessful)
                {
                    return await DependencyService.Get<INativeAuthService>().GetTokens();
                }
                else
                {
                    throw new UnauthorizedAccessException(result.Error, ex);
                }
            }
        }

        public async Task<(bool IsSucessful, string Error)> Login()
        {
            var urls = await _connectionService.GetUrls();
            return await DependencyService.Get<INativeAuthService>()
                .Login(urls.GetOIDCFullPathUrls());
        }

        public async Task Logout()
        {
            await DependencyService.Get<INativeAuthService>().LogOut();
        }

        public async Task<AuthenticationHeaderValue> GetAccessToken() => new AuthenticationHeaderValue("Bearer", (await GetToken()).accessToken);

    }
}
