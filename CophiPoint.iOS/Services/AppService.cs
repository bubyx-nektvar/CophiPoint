using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CophiPoint.Api;
using CophiPoint.Services;
using Foundation;
using OpenId.AppAuth;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CophiPoint.iOS.Services.AppService))]
namespace CophiPoint.iOS.Services
{
    public class AppService : INativeAppService
    {
        public void ExitApp()
        {
            Thread.CurrentThread.Abort();
        }
    }
}