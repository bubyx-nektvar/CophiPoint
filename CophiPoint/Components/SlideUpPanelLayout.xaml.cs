using CophiPoint.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static readonly BindableProperty PanelShownProperty = BindableProperty.Create(nameof(PanelShown), typeof(PanelState), typeof(SlideUpPanelLayout), PanelState.Hidden, BindingMode.TwoWay);

        public PanelState PanelShown
        {
            get => (PanelState)GetValue(PanelShownProperty);
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
        private int _lock = 0;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            MainLayout.HeightRequest = TranslationYMinimized;

            switch (PanelShown)
            {
                case PanelState.Hidden:
                    PanelLayout.TranslationY = TranslationYMinimized;
                    PanelLayout.GestureRecognizers.Add(ShowGesture);
                    break;
                case PanelState.Hiding:
                    PanelLayout.TranslationY = 0;
                    break;
                case PanelState.Showing:
                    PanelLayout.TranslationY = TranslationYMinimized;
                    break;
                case PanelState.Shown:
                    PanelLayout.TranslationY = 0;
                    break;
            }
        }

        async void Show(object sender, System.EventArgs e)
        {
            if (Interlocked.CompareExchange(ref _lock, 1, 0) == 0)
            {
                try
                {
                    PanelShown = PanelState.Showing;
                    PanelLayout.GestureRecognizers.Remove(ShowGesture);
                    await PanelLayout.TranslateTo(0, 0, 500, Easing.SinIn);
                    PanelShown = PanelState.Shown;
                }
                finally
                {
                    Interlocked.Exchange(ref _lock, 0);
                }
            }
        }

        async void Hide(object sender, System.EventArgs e)
        {
            PanelShown = PanelState.Hiding;
            PanelLayout.GestureRecognizers.Add(ShowGesture);
            await PanelLayout.TranslateTo(0, TranslationYMinimized, 500, Easing.SinIn);
            PanelShown = PanelState.Hidden;
        }
    }
}