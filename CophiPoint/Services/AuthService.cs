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

        public string GetToken()
        {
            //TODO
            return "";
        }

        public async Task Login()
        {
            var options = new OidcClientOptions
            {
                Authority = AuthConstants.AuthorityUri,
                ClientId = AuthConstants.ClientId,
                Scope = AuthConstants.Scope,
                RedirectUri = AuthConstants.RedirectUri,
                Flow = OidcClientOptions.AuthenticationFlow.Hybrid,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
            };

            var oidcClient = new OidcClient(options);
            var state = await oidcClient.PrepareLoginAsync();

            var response = await DependencyService.Get<ILoginBrowser>().LaunchBrowser(state.StartUrl);
            // HACK: Replace the RedirectURI, purely for UWP, with the current application callback URI.
            state.RedirectUri = AuthConstants.RedirectUri;
            var result = await oidcClient.ProcessResponseAsync(response, state);

            if (result.IsError)
            {
                Debug.WriteLine("\tERROR: {0}", result.Error);
                return;
            }

            //TODO store token
        }
    }
}
