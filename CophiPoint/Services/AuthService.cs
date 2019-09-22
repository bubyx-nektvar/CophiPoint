using CophiPoint.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint.Services
{
    public class AuthService
    {
        public bool IsLoggedIn
        {
            get
            {
                return DependencyService.Get<INativAuthService>().IsLogged;
            }
        }

        public Task<(string accessToken, string idToken)> GetToken()
        {
            return DependencyService.Get<INativAuthService>().GetTokens();
        }

        public Task<(bool IsSucessful, string Error)> Login() => DependencyService.Get<INativAuthService>().Login();

        public async Task Logout()
        {
            await DependencyService.Get<INativAuthService>().LogOut();
        }

        public async Task<AuthenticationHeaderValue> GetAccessToken() => new AuthenticationHeaderValue("Bearer", (await GetToken()).accessToken);
    }
}
