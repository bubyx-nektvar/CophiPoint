using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint.Pages
{
    public class HtmlPage : ContentPage
    {
        public HtmlPage(string url)
        {
            Title = url;
            Content = new StackLayout
            {
                Children = {
                    new WebView()
                    {
                        Source = new UrlWebViewSource()
                        {
                            Url = url
                        }
                    }
                }
            };
        }

        internal static Task<HtmlPage> FromUrl(string url)
        {
            return Task.FromResult(new HtmlPage(url));
        }
    }
}