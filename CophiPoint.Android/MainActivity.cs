using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using OpenId.AppAuth;
using Android.Content;
using Plugin.CurrentActivity;
using CophiPoint.Api;
using System.Threading.Tasks;

namespace CophiPoint.Droid
{
    [Activity(Label = "CophiPoint", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static Action<string> CustomUrlSchemeCallbackHandler { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            InitControls();

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void InitControls()
        {
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);
        }

        //public async Task Authorization()
        //{
        //    var authService = new AuthorizationService(global::Android.App.Application.Context);
        //    var configuration = await AuthorizationServiceConfiguration
        //        .FetchFromUrlAsync(global::Android.Net.Uri.Parse(AuthConstants.ConfigUrl));

        //    var authRequest = new AuthorizationRequest.Builder(
        //        configuration,
        //        AuthConstants.ClientId,
        //        ResponseTypeValues.Code,
        //        global::Android.Net.Uri.Parse(AuthConstants.RedirectUri)
        //    )
        //        .SetScope("openid profile email")
        //        .Build();

        //    Console.WriteLine("Making auth request to " + configuration.AuthorizationEndpoint);
        //    var intent = authService.GetAuthorizationRequestIntent(authRequest);

        //    StartActivityForResult(intent, RC_AUTH);
        //}

        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    if(requestCode == RC_AUTH)
        //    {
        //        AuthorizationResponse resp = AuthorizationResponse.FromIntent(data);
        //        AuthorizationException ex = AuthorizationException.FromIntent(data);
                
        //    }
        //    base.OnActivityResult(requestCode, resultCode, data);
        //}
    }
}