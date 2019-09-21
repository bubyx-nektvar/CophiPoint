using CophiPoint.Api;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint.Services
{
    public class AuthService
    {
        public bool IsLoggedIn { get; private set; }

        public Task<(string accessToken, string idToken)> GetToken()
        {
            return DependencyService.Get<INativAuthService>().GetTokens();
        }

        public async Task Login()
        {
            await DependencyService.Get<INativAuthService>().Login();
        }
    }
}
