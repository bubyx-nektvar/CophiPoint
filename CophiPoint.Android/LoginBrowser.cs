﻿
using Android.App;
using Android.Content;
using Android.Support.CustomTabs;
using CophiPoint.Api;
using CophiPoint.Droid;
using CophiPoint.Services;
using OpenId.AppAuth;
using Plugin.CurrentActivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginBrowser))]
namespace CophiPoint.Droid
{
    public class LoginBrowser : INativAuthService
    {
        public static LoginBrowser Instance;
        private readonly Context _context;
        private readonly AuthorizationService authService;
        private TaskCompletionSource<AuthState> taskCompletitionSource;

        private IClientAuthentication ClientAuthentication = new ClientSecretBasic(AuthConstants.ClientSecret);

        public LoginBrowser()
        {
            _context = CrossCurrentActivity.Current.Activity;
            authService = new AuthorizationService(_context);
            Instance = this;
        }

        public bool IsLogged => GetAuthState().IsAuthorized;

        public async Task<(string accessToken, string idToken)> GetTokens()
        {
            var authState = GetAuthState();
            if (authState.NeedsTokenRefresh)
            {
                var request = authState.CreateTokenRefreshRequest();
                authState = await PerformTokenRequest(request);
            }
            return (authState.AccessToken, authState.IdToken);
        }

        public async Task Login()
        {
            var configuration = await AuthorizationServiceConfiguration
                .FetchFromUrlAsync(global::Android.Net.Uri.Parse(AuthConstants.ConfigUrl));

            var authRequest = new AuthorizationRequest.Builder(
                configuration,
                AuthConstants.ClientId,
                ResponseTypeValues.Code,
                global::Android.Net.Uri.Parse(AuthConstants.RedirectUri)
            )
                .SetScope("openid profile email")
                .Build();

            Console.WriteLine("Making auth request to " + configuration.AuthorizationEndpoint);
            var intent = authService.GetAuthorizationRequestIntent(authRequest);

            taskCompletitionSource = new TaskCompletionSource<AuthState>();

            authService.PerformAuthorizationRequest(
                authRequest,
                AuthActivity.CreatePostAuthorizationIntent(
                    _context, 
                    authRequest),
                authService.CreateCustomTabsIntentBuilder().Build()
            );
            
            await taskCompletitionSource.Task;
        }

        internal void AuthActivityCreated(AuthorizationResponse resp, AuthorizationException ex)
        {
            var authState = GetAuthState();
            authState.Update(resp, ex);
            if(resp != null)
            {
                Console.WriteLine("Received AuthorizationResponse.");
                SetAuthState(authState);
                PerformTokenRequest(resp.CreateTokenExchangeRequest())
                    .ContinueWith(t => taskCompletitionSource.SetResult(t.Result));
            }
            else
            {
                Console.WriteLine("Auth failed: " + ex);
                taskCompletitionSource.SetResult(null);
            }
        }
        private Task<AuthState> PerformTokenRequest(TokenRequest request)
        {
            var tcs = new TaskCompletionSource<AuthState>();
            authService.PerformTokenRequest(request, ClientAuthentication,
                (TokenResponse tokenResponse, AuthorizationException authException)=>
                {
                    Console.WriteLine("Token request complete");
                    var authState = GetAuthState();

                    authState.Update(tokenResponse, authException);
                    SetAuthState(authState);

                    tcs.SetResult(authState);
                });
            return tcs.Task;
        }

        private AuthState GetAuthState()
        {
            var pref = _context.GetSharedPreferences(nameof(LoginBrowser), FileCreationMode.Private);
            var stateJson = pref.GetString(nameof(AuthState),null);
            if(stateJson != null)
            {
                return AuthState.JsonDeserialize(stateJson);
            }
            else
            {
                return new AuthState();
            }
        }

        private void SetAuthState(AuthState state)
        {
            var pref = _context.GetSharedPreferences(nameof(LoginBrowser), FileCreationMode.Private);
            pref.Edit()
                .PutString(nameof(AuthState), state.JsonSerializeString())
                .Apply();
        }
    }
}