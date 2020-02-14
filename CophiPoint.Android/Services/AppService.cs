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

[assembly: Dependency(typeof(CophiPoint.Droid.Services.AppService))]
namespace CophiPoint.Droid.Services
{
    public class AppService : INativeAppService
    {
        public void ExitApp()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}