using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OpenId.AppAuth;

namespace CophiPoint.Droid
{
    class Auth
    {
        //public async Task Authorize(Context context)
        //{
        //    var config = await AuthorizationServiceConfiguration.FetchFromUrlAsync(
        //    Uri.Parse("https://mojeid.cz/.well-known/openid-configuration/"));

        //    var request = new AuthorizationRequest.Builder(
        //        config,
        //        "aoaq8JhI2TvC",
        //        ResponseTypeValues.Code,
        //        Uri.Parse("bubyx.cz/connect")
        //        )
        //        .Build();

        //    AuthorizationService authService = new AuthorizationService(context);
        //    Intent authIntent = authService.GetAuthorizationRequestIntent(request);
        //    //TODO: startActivityForResult(authIntent, RC_AUTH);
        //}
    }
}