using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CophiPoint.Droid.Services;
using OpenId.AppAuth;

namespace CophiPoint.Droid
{
    //[Activity(Label = "CallbackInterceptorActivity")]
    //[IntentFilter(
    //    new[] { Intent.ActionView },
    //    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    //    DataScheme = "io.identitymodel.native",
    //    DataHost = "callback")]
    [Activity(Label = "Auth")]
    public class AuthActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AuthorizationResponse resp = AuthorizationResponse.FromIntent(Intent);
            AuthorizationException ex = AuthorizationException.FromIntent(Intent);

            AuthService.Instance.AuthActivityCreated(resp, ex);
            Finish();
        }
        public static PendingIntent CreatePostAuthorizationIntent(Context context, AuthorizationRequest request)
        {
            var intent = new Intent(context, typeof(AuthActivity));
            
            return PendingIntent.GetActivity(context, request.GetHashCode(), intent, 0);
        }
    }
}