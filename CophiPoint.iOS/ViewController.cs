using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using OpenId.AppAuth;
using CophiPoint.Api;
using System.Threading.Tasks;

namespace CophiPoint.iOS
{
    [Register("ViewController")]
    public partial class ViewController : UIViewController, IAuthStateChangeDelegate, IAuthStateErrorDelegate
    {
        public ViewController() : base()
        { }

        protected ViewController(IntPtr handle)
            : base(handle)
        { }


        // Authorization code flow using OIDAuthState automatic code exchanges.
        public async Task<(bool, string)> AuthWithAutoCodeExchange()
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
                var request = new AuthorizationRequest(configuration, AuthConstants.ClientId, AuthConstants.ScopesArray, redirectURI, ResponseType.Code, null);
                // performs authentication request
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                Console.WriteLine($"Initiating authorization request with scope: {request.Scope}");

                var tcl = new TaskCompletionSource<(bool,string)>();

                appDelegate.CurrentAuthorizationFlow = AuthState
                    .PresentAuthorizationRequest(request, this, (authState, error) =>
                {
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
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error retrieving discovery document: {ex}");
                AuthService.ClearState();
                return (false, ex.Message);
            }
        }


        public void DidChangeState(AuthState state)
        {
            AuthService.SaveState(state);
        }

        public void DidEncounterAuthorizationError(AuthState state, NSError error)
        {
            Console.WriteLine($"Received authorization error: {error}.");
        }


        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }
    }
}