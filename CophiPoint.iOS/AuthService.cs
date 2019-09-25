using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CophiPoint.Api;
using CophiPoint.Services;
using Foundation;
using OpenId.AppAuth;
using SafariServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CophiPoint.iOS.AuthService))]
namespace CophiPoint.iOS
{
    public class AuthService : INativAuthService, IAuthStateChangeDelegate, IAuthStateErrorDelegate
    {

        public bool IsLogged
        {
            get
            {
                var state = LoadState();
                return state != null 
                    && state.IsAuthorized 
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

        public IntPtr Handle { get; }

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

        public async Task<(bool IsSucessful, string Error)> Login()
        {
            Console.WriteLine(nameof(Login));
            return await AuthWithAutoCodeExchange();
        }

        public Task LogOut()
        {
            ClearState();
            return Task.CompletedTask;
        }
        private async Task<AuthState> PerformRefresh(AuthState authState)
        {
            Console.WriteLine(nameof(PerformRefresh));
            var request = authState.TokenRefreshRequest();
            try
            {
                var tokenResponse = await AuthorizationService.PerformTokenRequestAsync(request);
                Console.WriteLine($"Received token response with accessToken: {tokenResponse.AccessToken}");

                authState.Update(tokenResponse, null);
            }
            catch (NSErrorException ex)
            {
                authState.Update(ex.Error);

                Console.WriteLine($"Token exchange error: {ex}");
            }
            return authState;
        }

        public async Task<(bool, string)> AuthWithAutoCodeExchange()
        {
            Console.WriteLine(nameof(AuthWithAutoCodeExchange));
            var discoveryUri = new NSUrl(AuthConstants.ConfigUrl);
            var redirectURI = new NSUrl(AuthConstants.RedirectUri);

            Console.WriteLine($"Fetching configuration for issuer: {discoveryUri}");

            try
            {
                // discovers endpoints
                var configuration = await AuthorizationService.DiscoverServiceConfigurationForDiscoveryAsync(discoveryUri);

                Console.WriteLine($"Got configuration: {configuration}");

                // builds authentication request
                var request = new AuthorizationRequest(configuration, AuthConstants.ClientId, AuthConstants.ClientSecret, AuthConstants.ScopesArray, redirectURI, ResponseType.Code, null);
                // performs authentication request
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                Console.WriteLine($"Initiating authorization request with scope: {request.Scope}");

                var tcl = new TaskCompletionSource<(bool, string)>();

                appDelegate.CurrentAuthorizationFlow = AuthState
                    .PresentAuthorizationRequest(request, appDelegate.Window.RootViewController, (authState, error) =>
                    {
                        Console.WriteLine(nameof(AuthState.PresentAuthorizationRequest) + "Done");
                        if (authState != null)
                        {
                            AuthService.SaveState(authState);
                            Console.WriteLine($"Got authorization tokens. Access token: {authState.LastTokenResponse.AccessToken}");
                            tcl.SetResult((true, null));
                        }
                        else
                        {
                            Console.WriteLine($"Authorization error: {error.LocalizedDescription}");
                            AuthService.ClearState();
                            tcl.SetResult((false, error.LocalizedDescription));
                        }
                    });
                return await tcl.Task;
                //return (false, "test");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error retrieving discovery document: {ex}");
                AuthService.ClearState();
                return (false, ex.Message);
            }
        }


        // Authorization code flow without a the code exchange (need to call codeExchange manually)
        async Task<(bool, string)> AuthNoCodeExchange()
        {
            var discoveryUri = new NSUrl(AuthConstants.ConfigUrl);
            var redirectURI = new NSUrl(AuthConstants.RedirectUri);

            Console.WriteLine($"Fetching configuration for issuer: {discoveryUri}");

            try
            {
                // discovers endpoints
                var configuration = await AuthorizationService.DiscoverServiceConfigurationForDiscoveryAsync(discoveryUri);

                Console.WriteLine($"Got configuration: {configuration}");
                
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
                Console.WriteLine($"Initiating authorization request: {request}");
                appDelegate.CurrentAuthorizationFlow = AuthorizationService.PresentAuthorizationRequest(request, appDelegate.Window.RootViewController, 
                    (authorizationResponse, error) =>
                {
                    Console.WriteLine(nameof(AuthorizationService.PresentAuthorizationRequest) + "Done");
                    if (authorizationResponse != null)
                    {
                        var authState = new AuthState(authorizationResponse);
                        AuthService.SaveState(authState);
                        Console.WriteLine($"Got authorization tokens. Access token: {authState.LastTokenResponse.AccessToken}");
                        tcl.SetResult((true, null));
                    }
                    else
                    {
                        Console.WriteLine($"Authorization error: {error.LocalizedDescription}");
                        AuthService.ClearState();
                        tcl.SetResult((false, error.LocalizedDescription));
                    }
                });
                return await tcl.Task;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error retrieving discovery document: {ex}");
                AuthService.ClearState();
                return (false, ex.Message);
            }
        }

        // NSCoding key for the authState property.
        public static NSString kAppAuthExampleAuthStateKey = (NSString)"authState";

        internal static void SaveState(AuthState state)
        {
            Console.WriteLine(nameof(SaveState));
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

            Console.WriteLine(nameof(SaveState) +"Done");
        }

        // Loads the OIDAuthState from NSUSerDefaults.
        internal static AuthState LoadState()
        {
            Console.WriteLine(nameof(LoadState));
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
            Console.WriteLine(nameof(DidChangeState));
            SaveState(state);
        }

        public void DidEncounterAuthorizationError(AuthState state, NSError error)
        {
            Console.WriteLine(nameof(DidEncounterAuthorizationError));
            Console.WriteLine($"Received authorization error: {error}.");
        }

        public void Dispose()
        {
            Console.WriteLine(nameof(Dispose));
        }
    }
}