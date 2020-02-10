using CophiPoint.Pages;
using CophiPoint.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using CophiPoint.Api;

namespace CophiPoint
{
    public partial class App : Application
    {
        public readonly OrderManager OrderManager;

        public readonly ProductManager ProductManager;

        public readonly ApiConnectionService ConnectionService;

        public readonly AuthService AuthService;

        public App()
        {
            InitializeComponent();
            
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                GeneralResources.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }

            ConnectionService = new ApiConnectionService();
            AuthService = new AuthService(ConnectionService);
            if (false)
            {
                var restService = new TestRestService();
                ProductManager = new ProductManager(restService);
                OrderManager = new OrderManager(restService, ProductManager, AuthService);
            }
            else
            {
                ProductManager = new ProductManager(new ProductsApi(ConnectionService));
                OrderManager = new OrderManager(new UserApi(ConnectionService, AuthService), ProductManager, AuthService);
            }


            MainPage = new NavigationPage(new LoginPage());
        }

        public async Task Reload()
        {
            await ProductManager.Load();
            await OrderManager.Load();
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
            if (AuthService.IsLoggedIn)
            {
                await Reload();
            }
            // Handle when your app resumes
        }
    }
}
