using CophiPoint.Api;
using CophiPoint.Api.Models;
using CophiPoint.Services;
using CophiPoint.Services.Implementation;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly HtmlManager ConnectionService;

        public MainPage(HtmlManager connectionService)
        {
            ConnectionService = connectionService;
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var page = await ConnectionService.GetPageAsync(GeneralResources.InfoTitle, u => u.General);
            await Navigation.PushAsync(page);
        }
    }
}
