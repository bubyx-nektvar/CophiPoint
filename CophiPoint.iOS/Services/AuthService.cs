using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CophiPoint.Api;
using CophiPoint.Helpers;
using CophiPoint.Services;
using Foundation;
using OpenId.AppAuth;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CophiPoint.iOS.Services.AuthService))]
namespace CophiPoint.iOS.Services
{
    public class AuthService : INativeAuthService, IAuthStateChangeDelegate
    {
        public IntPtr Handle { get; }

        public bool IsLogged
        {
            get
            {
                var state = LoadState();
                return (state?.IsAuthorized ?? false)
                    && (
                        (!string.IsNullOrWhiteSpace(state.RefreshToken)) 
                        ||
                        (
                            state.LastTokenResponse != null
                            &&
                            state.LastTokenResponse.AccessTokenExpirationDate.Compare(NSDate.Now) == NSComparisonResult.Descending
                        )
                );
            }
        }


        public async Task<(string accessToken, string idToken)> GetTokens()
        {
            var authState = LoadState();
            if ((authState.LastTokenResponse.AccessTokenExpirationDate.Compare(NSDate.Now) == NSComparisonResult.Descending))
            {
                try
                {
                    authState = await PerformRefresh(authState);
                    SaveState(authState);
                }
                catch (Exception ex)
                {
                    throw new UnauthorizedAccessException("refresh failed", ex);
                }
            }
            
            return (authState.LastTokenResponse.AccessToken, authState.LastTokenResponse.IdToken);
        }

        public async Task<(bool IsSucessful, string Error)> Login(Api.Urls.OIDCUrls urls)
        {
            MicroLogger.LogDebug(nameof(Login));
            return await AuthWithAutoCodeExchange(urls);
        }

        public Task LogOut()
        {
            ClearState();
            return Task.CompletedTask;
        }
        private async Task<AuthState> PerformRefresh(AuthState authState)
        {
            MicroLogger.LogDebug(nameof(PerformRefresh));

            var request = authState.TokenRefreshRequest();
            try
            {
                var tokenResponse = await AuthorizationService.PerformTokenRequestAsync(request);
                MicroLogger.LogDebug($"Received token response with accessToken: {tokenResponse.AccessToken}");

                authState.Update(tokenResponse, null);
            }
            catch (NSErrorException ex)
            {
                authState.Update(ex.Error);

                MicroLogger.LogError($"Token exchange error: {ex}");
            }
            return authState;
        }
        
        private NSUrl ToUrl(string url) =>new NSUrl(url);

        public async Task<(bool, string)> AuthWithAutoCodeExchange(Urls.OIDCUrls urls)
        {
            MicroLogger.LogDebug(nameof(AuthWithAutoCodeExchange));
            var redirectURI = new NSUrl(AuthConstants.RedirectUri);

            try
            {
                // discovers endpoints
                var configuration = new ServiceConfiguration(ToUrl(urls.Authorization), ToUrl(urls.Token));

                MicroLogger.LogDebug($"Got configuration: {configuration}");

                // builds authentication request
                var request = new AuthorizationRequest(configuration, AuthConstants.ClientId, AuthConstants.ClientSecret, AuthConstants.ScopesArray, redirectURI, ResponseType.Code, null);
                // performs authentication request
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                MicroLogger.LogDebug($"Initiating authorization request with scope: {request.Scope}");

                var tcl = new TaskCompletionSource<(bool, string)>();

                appDelegate.CurrentAuthorizationFlow = AuthState
                    .PresentAuthorizationRequest(request, appDelegate.Window.RootViewController, (authState, error) =>
                    {
                        MicroLogger.LogDebug(nameof(AuthState.PresentAuthorizationRequest) + "Done");
                        if (authState != null)
                        {
                            AuthService.SaveState(authState);
                            MicroLogger.LogDebug($"Got authorization tokens. Access token: {authState.LastTokenResponse.AccessToken}");
                            tcl.SetResult((true, null));
                        }
                        else
                        {
                            MicroLogger.LogError($"Authorization error: {error.LocalizedDescription}");
                            AuthService.ClearState();
                            tcl.SetResult((false, error.LocalizedDescription));
                        }
                    });
                return await tcl.Task;
                //return (false, "test");
            }
            catch (Exception ex)
            {

                MicroLogger.LogError($"Error retrieving discovery document: {ex}");
                AuthService.ClearState();
                return (false, ex.Message);
            }
        }


        // Authorization code flow without a the code exchange (need to call codeExchange manually)
        async Task<(bool, string)> AuthNoCodeExchange(Urls.OIDCUrls urls)
        {
            var redirectURI = new NSUrl(AuthConstants.RedirectUri);


            try
            {
                // discovers endpoints
                var configuration = new ServiceConfiguration(ToUrl(urls.Authorization), ToUrl(urls.Token));

                MicroLogger.LogDebug($"Got configuration: {configuration}");

                // builds authentication request
                AuthorizationRequest request = new AuthorizationRequest(
                    configuration,
                    AuthConstants.ClientId,
                    AuthConstants.ClientSecret,
                    AuthConstants.ScopesArray,
                    redirectURI,
                    ResponseType.Code,
                    null);
                // performs authentication request
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

                var tcl = new TaskCompletionSource<(bool, string)>();
                MicroLogger.LogDebug($"Initiating authorization request: {request}");
                appDelegate.CurrentAuthorizationFlow = AuthorizationService.PresentAuthorizationRequest(request, appDelegate.Window.RootViewController,
                    (authorizationResponse, error) =>
                    {
                        MicroLogger.LogDebug(nameof(AuthorizationService.PresentAuthorizationRequest) + "Done");
                        if (authorizationResponse != null)
                        {
                            var authState = new AuthState(authorizationResponse);
                            AuthService.SaveState(authState);
                            MicroLogger.LogDebug($"Got authorization tokens. Access token: {authState.LastTokenResponse.AccessToken}");
                            tcl.SetResult((true, null));
                        }
                        else
                        {
                            MicroLogger.LogError($"Authorization error: {error.LocalizedDescription}");
                            AuthService.ClearState();
                            tcl.SetResult((false, error.LocalizedDescription));
                        }
                    });
                return await tcl.Task;
            }
            catch (Exception ex)
            {

                MicroLogger.LogError($"Error retrieving discovery document: {ex}");
                AuthService.ClearState();
                return (false, ex.Message);
            }
        }


        // NSCoding key for the authState property.
        public static NSString kAppAuthExampleAuthStateKey = (NSString)"authState";
        
        internal static void SaveState(AuthState state)
        {
            MicroLogger.LogDebug(nameof(SaveState));
            // for production usage consider using the OS Keychain instead
            if (state != null)
            {
                var archivedAuthState = NSKeyedArchiver.ArchivedDataWithRootObject(state);
                NSUserDefaults.StandardUserDefaults[kAppAuthExampleAuthStateKey] = archivedAuthState;
            }
            else
            {
                NSUserDefaults.StandardUserDefaults.RemoveObject(kAppAuthExampleAuthStateKey);
            }
            NSUserDefaults.StandardUserDefaults.Synchronize();

            MicroLogger.LogDebug(nameof(SaveState) + "Done");
        }

        // Loads the OIDAuthState from NSUSerDefaults.
        internal static AuthState LoadState()
        {
            MicroLogger.LogDebug(nameof(LoadState));
            // loads OIDAuthState from NSUSerDefaults
            var archivedAuthState = (NSData)NSUserDefaults.StandardUserDefaults[kAppAuthExampleAuthStateKey];
            if (archivedAuthState != null)
            {
                return (AuthState)NSKeyedUnarchiver.UnarchiveObject(archivedAuthState);
            }
            return null;
        }
        internal static void ClearState()
        {
            SaveState(null);
        }

        public void DidChangeState(AuthState state)
        {
            MicroLogger.LogDebug(nameof(DidChangeState));
            SaveState(state);
        }
  
        public void DidEncounterAuthorizationError(AuthState state, NSError error)
        {
            MicroLogger.LogDebug(nameof(DidEncounterAuthorizationError));
            MicroLogger.LogDebug($"Received authorization error: {error}.");
        }
  
        public void Dispose()
        {
            MicroLogger.LogDebug(nameof(Dispose));
        }
    }
}