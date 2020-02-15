using CophiPoint.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SlideUpPanelLayout : ContentView
    {
        public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create(nameof(HeaderContent), typeof(View), typeof(SlideUpPanelLayout), null,
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {

                    ((SlideUpPanelLayout)bindableObject).HeaderHolder.Content = (View)newValue;
                });

        public View HeaderContent
        {
            get => (View)GetValue(HeaderContentProperty);
            set => SetValue(HeaderContentProperty, value);
        }
        
        public static readonly BindableProperty MainContentProperty = BindableProperty.Create(nameof(MainContent), typeof(View), typeof(SlideUpPanelLayout), null,
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {
                    ((SlideUpPanelLayout)bindableObject).MainLayout.Content = (View)newValue;
                });

        public View MainContent
        {
            get => (View)GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }

        public static readonly BindableProperty BodyContentProperty = BindableProperty.Create(nameof(BodyContent), typeof(View), typeof(SlideUpPanelLayout), null,
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {
                    ((SlideUpPanelLayout)bindableObject).BodyHolder.Content = (View)newValue;
                });


        public View BodyContent
        {
            get => (View)GetValue(BodyContentProperty);
            set => SetValue(BodyContentProperty, value);
        }

        public static readonly BindableProperty PanelShownProperty = BindableProperty.Create(nameof(PanelShown), typeof(bool?), typeof(SlideUpPanelLayout), false, BindingMode.TwoWay);

        public bool? PanelShown
        {
            get => (bool?)GetValue(PanelShownProperty);
            set => SetValue(PanelShownProperty, value);
        }

        private readonly TapGestureRecognizer ShowGesture = new TapGestureRecognizer();

        public SlideUpPanelLayout()
        {
            InitializeComponent();
            PanelLayout.TranslationY = 2000;
            ShowGesture.Tapped += Show;
        }

        private double TranslationYMinimized => Height * 0.75;
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            MainLayout.HeightRequest = TranslationYMinimized;
            
            if (PanelShown == null || (!PanelShown.Value))
            {
                PanelLayout.TranslationY = TranslationYMinimized;
                PanelLayout.GestureRecognizers.Add(ShowGesture);
            }
            else
            {
                PanelLayout.TranslationY = 0;
            }
        }

        async void Show(object sender, System.EventArgs e)
        {
            PanelShown = true;
            PanelLayout.GestureRecognizers.Remove(ShowGesture);
            await PanelLayout.TranslateTo(0, 0, 500, Easing.SinIn);
            PanelShown = true;
        }

        async void Hide(object sender, System.EventArgs e)
        {
            PanelShown = false;
            PanelLayout.GestureRecognizers.Add(ShowGesture);
            await PanelLayout.TranslateTo(0, TranslationYMinimized, 500, Easing.SinIn);
            PanelShown = false;
        }
    }
}