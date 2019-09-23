
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

[assembly: Dependency(typeof(CophiPoint.Droid.AuthService))]
namespace CophiPoint.Droid
{
    public class AuthService : INativAuthService
    {
        public static AuthService Instance;
        private readonly Context _context;
        private readonly AuthorizationService authService;
        private TaskCompletionSource<AuthState> taskCompletitionSource;

        private IClientAuthentication ClientAuthentication = new ClientSecretBasic(AuthConstants.ClientSecret);

        public AuthService()
        {
            _context = CrossCurrentActivity.Current.Activity;
            authService = new AuthorizationService(_context);
            Instance = this;
        }

        public bool IsLogged { 
            get {
                var state = GetAuthState();
                return state.IsAuthorized && ((!string.IsNullOrWhiteSpace(state.RefreshToken)) || (!state.NeedsTokenRefresh));
            }
        }

        public async Task<(string accessToken, string idToken)> GetTokens()
        {
            var authState = GetAuthState();
            if (authState.NeedsTokenRefresh)
            {
                try
                {
                    var request = authState.CreateTokenRefreshRequest();
                    authState = await PerformTokenRequest(request);
                }
                catch (AuthorizationException ex)
                {
                    throw new UnauthorizedAccessException("refresh failed", ex);
                }
            }
            return (authState.AccessToken, authState.IdToken);
        }

        public async Task<(bool IsSucessful, string Error)> Login()
        {
            var configuration = await AuthorizationServiceConfiguration
                .FetchFromUrlAsync(global::Android.Net.Uri.Parse(AuthConstants.ConfigUrl));

            var authRequestBuilder = new AuthorizationRequest.Builder(
                configuration,
                AuthConstants.ClientId,
                ResponseTypeValues.Code,
                global::Android.Net.Uri.Parse(AuthConstants.RedirectUri)
            ).SetScope(AuthConstants.Scope);
            
            if (AuthConstants.Scope.Contains("offline_access")) 
            {
                authRequestBuilder = authRequestBuilder.SetPrompt("consent");
            }
            var authRequest = authRequestBuilder.Build();

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

            var state = await taskCompletitionSource.Task;
            if (state.AuthorizationException != null)
            {
                return (false, state.AuthorizationException.ErrorDescription);
            }
            else
            {
                return (true, null);
            }
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
                taskCompletitionSource.SetResult(authState);
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
                    if (tokenResponse.AccessTokenExpirationTime == null) {
                        tokenResponse.AccessTokenExpirationTime = new Java.Lang.Long(
                            DateTimeOffset.Now
                                .AddMinutes(AuthConstants.ExpirationPeriodMinutes)
                                .ToUnixTimeMilliseconds()
                        );
                    }

                    authState.Update(tokenResponse, authException);
                    SetAuthState(authState);

                    tcs.SetResult(authState);
                });
            return tcs.Task;
        }

        private AuthState GetAuthState()
        {
            var pref = _context.GetSharedPreferences(nameof(AuthService), FileCreationMode.Private);
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
            var pref = _context.GetSharedPreferences(nameof(AuthService), FileCreationMode.Private);
            pref.Edit()
                .PutString(nameof(AuthState), state.JsonSerializeString())
                .Apply();
        }
        private void RemoveAuthState()
        {
            var pref = _context.GetSharedPreferences(nameof(AuthService), FileCreationMode.Private);
            pref.Edit()
                .Remove(nameof(AuthState))
                .Apply();
        }

        public Task LogOut()
        {
            RemoveAuthState();
            return Task.CompletedTask;
        }
    }
}