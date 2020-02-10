using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CophiPoint.Api;

namespace CophiPoint.Pages
{
    public class HtmlPage : ContentPage
    {
        public HtmlPage(string url, string title)
        {
            Title = title;
            Content = new StackLayout
            {
                Children = {
                    new WebView()
                    {

                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions =LayoutOptions.FillAndExpand,
                        Source = new UrlWebViewSource()
                        {
                            Url = url
                        }
                    }
                }
            };
        }

        internal static Task<HtmlPage> FromUrl(string url, string title)
        {
            return Task.FromResult(new HtmlPage(url, title));
        }
        public static Task<HtmlPage> GetInfoPage(Urls.InfoUrls urls)
        {
            return HtmlPage.FromUrl(urls.General, GeneralResources.InfoTitle);
        }
        public static Task<HtmlPage> GetPaymentPage(Urls.InfoUrls urls)
        {
            return HtmlPage.FromUrl(urls.Payment, GeneralResources.PayInfoTitle);
        }
    }
}