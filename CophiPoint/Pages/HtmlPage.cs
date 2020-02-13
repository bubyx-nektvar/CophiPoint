using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CophiPoint.Api;
using CophiPoint.Services.Implementation;

namespace CophiPoint.Pages
{
    public class HtmlPage : ContentPage
    {
        public HtmlPage(string content, string title)
        {
            Title = title;
            Content = new StackLayout
            {
                Children = {
                    new WebView()
                    {

                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions =LayoutOptions.FillAndExpand,
                        Source = new HtmlWebViewSource()
                        {
                            Html = content
                        }
                    }
                }
            };
        }
    }
}