using CophiPoint.Pages;
using CophiPoint.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using CophiPoint.Api;
using TinyIoC;
using CophiPoint.Services.Implementation;
using CophiPoint.Helpers;

namespace CophiPoint
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                GeneralResources.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }

            var container = TinyIoCContainer.Current;
            container.Register<IHttpRestService, ApiConnectionService>().AsSingleton();
            container.Register<AuthService>().AsSingleton();

            if (false)
            {
                container.Register<TestRestService>().AsSingleton();
            }
            else
            {
                container.Register<IProductService, ProductsApi>().AsSingleton();
                container.Register<IOrderService, UserApi>().AsSingleton();
            }
            container.Register<ICacheService,CacheService>();
            container.Register<HtmlManager>();
            container.Register<ProductManager>().AsSingleton();
            container.Register<OrderManager>().AsSingleton();

            MainPage = new NavigationPage(
                TinyIoCContainer.Current.Resolve<LoginPage>()
            );
        }

        public async Task Reload()
        {
            MicroLogger.LogDebug("Reloading data sets from Server");
            await TinyIoCContainer.Current.Resolve<OrderManager>().Load();
            await TinyIoCContainer.Current.Resolve<ProductManager>().Load();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override async void OnResume()
        {
            if (TinyIoCContainer.Current.Resolve<AuthService>().IsLoggedIn)
            {
                await Reload();
            }
            // Handle when your app resumes
        }
    }
}
