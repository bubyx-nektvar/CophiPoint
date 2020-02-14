
using Android.App;
using Android.Content;
using Android.Support.CustomTabs;
using CophiPoint.Api;
using CophiPoint.Droid;
using CophiPoint.Helpers;
using CophiPoint.Services;
using OpenId.AppAuth;
using Plugin.CurrentActivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(CophiPoint.Droid.Services.AuthService))]
namespace CophiPoint.Droid.Services
{
    public class AuthService : INativeAuthService
    {
        public static AuthService Instance;
        private readonly Context _context;
        private readonly AuthorizationService authService;
        private TaskCompletionSource<AuthState> taskCompletitionSource;

        private readonly IClientAuthentication ClientAuthentication = new ClientSecretBasic(AuthConstants.ClientSecret);

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
        private static global::Android.Net.Uri ToUrl(string url) => global::Android.Net.Uri.Parse(url);

        public async Task<(bool IsSucessful, string Error)> Login(Urls.OIDCUrls urls)
        {
            var configuration = new AuthorizationServiceConfiguration(
                ToUrl(urls.Authorization), 
                ToUrl(urls.Token)
                );

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

            MicroLogger.LogDebug("Making auth request to " + configuration.AuthorizationEndpoint);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var intent = authService.GetAuthorizationRequestIntent(authRequest);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

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
                MicroLogger.LogDebug("Received AuthorizationResponse.");
                SetAuthState(authState);
                PerformTokenRequest(resp.CreateTokenExchangeRequest())
                    .ContinueWith(t => taskCompletitionSource.SetResult(t.Result));
            }
            else
            {
                MicroLogger.LogError("Auth failed: " + ex);
                taskCompletitionSource.SetResult(authState);
            }
        }

        private Task<AuthState> PerformTokenRequest(TokenRequest request)
        {
            MicroLogger.LogDebug($"Request token {request.AuthorizationCode}");
               var tcs = new TaskCompletionSource<AuthState>();
            authService.PerformTokenRequest(request, ClientAuthentication,
                (TokenResponse tokenResponse, AuthorizationException authException)=>
                {
                    if (tokenResponse != null)
                    {
                        MicroLogger.LogDebug("Token request complete");
                        var authState = GetAuthState();
                        if (tokenResponse?.AccessTokenExpirationTime == null)
                        {
                            tokenResponse.AccessTokenExpirationTime = new Java.Lang.Long(
                                DateTimeOffset.Now
                                    .AddMinutes(AuthConstants.ExpirationPeriodMinutes)
                                    .ToUnixTimeMilliseconds()
                            );
                        }

                        authState.Update(tokenResponse, authException);
                        SetAuthState(authState);

                        tcs.SetResult(authState);
                    }
                    else
                    {
                        throw authException;
                    }
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