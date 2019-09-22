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

        public Task<(bool IsSucessful, string Error)> Login() => DependencyService.Get<INativAuthService>().Login();

        public async Task Logout()
        {
            await DependencyService.Get<INativAuthService>().LogOut();
        }

        public async Task<AuthenticationHeaderValue> GetAccessToken() => new AuthenticationHeaderValue("Bearer", (await GetToken()).accessToken);
    }
}
