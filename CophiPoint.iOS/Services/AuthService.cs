using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CophiPoint.Api;
using CophiPoint.Services;
using Foundation;
using OpenId.AppAuth;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CophiPoint.iOS.Services.AuthService))]
namespace CophiPoint.iOS.Services
{
    public class AuthService : INativAuthService
    {

        public bool IsLogged
        {
            get
            {
                var state = LoadState();
                return state.IsAuthorized 
                    && (
                        (!string.IsNullOrWhiteSpace(state.RefreshToken)) 
                        || 
                        (state.LastTokenResponse.AccessTokenExpirationDate.Compare(NSDate.Now) == NSComparisonResult.Descending)
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
            rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            var authController = new ViewController();
            var tsc = new TaskCompletionSource<object>();
            rootViewController.PresentViewController(authController, false, ()=> {
                tsc.SetResult(null);
            });
            await tsc.Task;
            return await authController.AuthWithAutoCodeExchange(urls);
        }

        public Task LogOut()
        {
            ClearState();
            return Task.CompletedTask;
        }
        private async Task<AuthState> PerformRefresh(AuthState authState)
        {
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

        // NSCoding key for the authState property.
        public static NSString kAppAuthExampleAuthStateKey = (NSString)"authState";
        private UIViewController rootViewController;

        internal static void SaveState(AuthState state)
        {
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
        }

        // Loads the OIDAuthState from NSUSerDefaults.
        internal static AuthState LoadState()
        {
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
    }
}