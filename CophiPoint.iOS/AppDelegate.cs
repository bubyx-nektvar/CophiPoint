using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Foundation;
using OpenId.AppAuth;
using UIKit;
using Xamarin.Forms;

namespace CophiPoint.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // The authorization flow session which receives the return URL from SFSafariViewController.
        public IAuthorizationFlowSession CurrentAuthorizationFlow { get; set; }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("CarouselView_Experimental", "IndicatorView_Experimental");

            Forms.Init();

            InitControls();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        private void InitControls()
        {
            CachedImageRenderer.Init();
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var ignore = typeof(SvgCachedImage);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }

        // Handles inbound URLs. Checks if the URL matches the redirect URI for a pending
        // AppAuth authorization request.
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            // Sends the URL to the current authorization flow (if any) which will process it if it relates to
            // an authorization response.
            if (CurrentAuthorizationFlow?.ResumeAuthorizationFlow(url) == true)
            {
                return true;
            }

            // Your additional URL handling (if any) goes here.

            return false;
        }
    }
}
