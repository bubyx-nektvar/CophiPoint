using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CophiPoint.Services;
using Xamarin.Android.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(CophiPoint.Droid.Services.NativeHttpService))]
namespace CophiPoint.Droid.Services
{
    public class NativeHttpService : INativeHttpService
    {
        public HttpMessageHandler GetNativeHandler() => new AndroidClientHandler();
    }
}