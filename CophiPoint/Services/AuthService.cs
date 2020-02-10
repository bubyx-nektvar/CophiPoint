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

namespace CophiPoint.Services
{
    public class AuthService
    {
        private ApiConnectionService _connectionService;

        public AuthService(ApiConnectionService initService)
        {
            this._connectionService = initService;
        }

        public bool IsLoggedIn
        {
            get
            {
                return DependencyService.Get<INativAuthService>().IsLogged;
            }
        }

        public async Task<(string accessToken, string idToken)> GetToken()
        {
            try
            {
                return await DependencyService.Get<INativAuthService>().GetTokens();
            }
            catch (UnauthorizedAccessException ex)
            {
                var result = await Login();
                if (result.IsSucessful)
                {
                    return await DependencyService.Get<INativAuthService>().GetTokens();
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
            return await DependencyService.Get<INativAuthService>()
                .Login(urls.GetOIDCFullPathUrls());
        }

        public async Task Logout()
        {
            await DependencyService.Get<INativAuthService>().LogOut();
        }

        public async Task<AuthenticationHeaderValue> GetAccessToken() => new AuthenticationHeaderValue("Bearer", (await GetToken()).accessToken);

        public async Task<TResponse> GetAuthorizedAsync<TResponse>(Func<Urls,string> relativePathSelector)
        {
            var token = await GetAccessToken();

            var response = await _connectionService.SendAsync(HttpMethod.Get, relativePathSelector, message =>
            {
                message.Headers.Authorization = token;
            });
            return await response.ParseResultOrFail<TResponse>();
        }

        public async Task<HttpResponseMessage> PostAuthorizedAsync<T>(T contentObject, Func<Urls, string> relativePathSelector)
        {

            var token = await GetAccessToken();
            var json = JsonConvert.SerializeObject(contentObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _connectionService.SendAsync(HttpMethod.Post, relativePathSelector, message =>
            {
                message.Headers.Authorization = token;
                message.Content = content;
             });
        }

    }
}
