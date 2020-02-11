using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using CophiPoint.Services;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CophiPoint.iOS.Services.NativeHttpService))]
namespace CophiPoint.iOS.Services
{
    public class NativeHttpService : INativeHttpService
    {
        public HttpMessageHandler GetNativeHandler() => new NSUrlSessionHandler();
    }
}