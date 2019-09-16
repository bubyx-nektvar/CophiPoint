using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.ComponentModel;
using System.Drawing;
using CophiPoint.Components;
using CophiPoint.iOS.Renderers;

[assembly: ExportRenderer(typeof(CarouselViewLayout), typeof(CarouselLayoutRenderer))]

namespace CophiPoint.iOS.Renderers
{
    public class CarouselLayoutRenderer : ScrollViewRenderer
    {
        UIScrollView _native;
        CarouselViewLayout LayoutElement => (CarouselViewLayout)Element;

        public CarouselLayoutRenderer()
        {
            PagingEnabled = true;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null) return;

            _native = (UIScrollView)NativeView;
            _native.Scrolled += NativeScrolled;
            e.NewElement.PropertyChanged += ElementPropertyChanged;
        }

        void NativeScrolled(object sender, EventArgs e)
        {
            var targetIndex = Convert.ToInt32(System.Math.Round(_native.ContentOffset.X));
            LayoutElement.SelectedIndex = System.Math.Min(LayoutElement.Children.Count - 1, System.Math.Max(0, targetIndex));
            ScrollToSelection(true);
        }
        void ScrollToSelection(bool animate)
        {
            if (Element == null) return;
            var targetX = _native.Bounds.Width * LayoutElement.SelectedIndex;
            _native.SetContentOffset(new CoreGraphics.CGPoint(targetX, _native.ContentOffset.Y), animate);
        }
        void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CarouselViewLayout.SelectedIndexProperty.PropertyName && !Dragging)
            {
                ScrollToSelection(false);
            }
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);
            ScrollToSelection(false);
        }
    }
}

