using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Reflection;
using Java.Lang;
using System.Timers;
using Android.Widget;
using Android.Views;
using System.ComponentModel;
using Android.Graphics;
using CophiPoint.Components;
using CophiPoint.Android.Renderers;
using System;
using Android.Support.V4.Widget;

[assembly: ExportRenderer(typeof(CarouselViewLayout), typeof(CarouselLayoutRenderer))]

namespace CophiPoint.Android.Renderers
{
    public class CarouselLayoutRenderer : ScrollViewRenderer
    {
        float _direction;
        bool _motionDown;
        Timer _swipeTimer;
        //Timer _scrollStopTimer;
        HorizontalScrollView _scrollView;

        public CarouselViewLayout LayoutElement => (CarouselViewLayout)Element;

        public CarouselLayoutRenderer(global::Android.Content.Context context)
            :base(context)
        { }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;

            _swipeTimer = new Timer(50) { AutoReset = false };
            _swipeTimer.Elapsed += (object sender, ElapsedEventArgs args) => SnapScroll();

            e.NewElement.PropertyChanged += ElementPropertyChanged;
        }

        void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Renderer")
            {
                _scrollView = (HorizontalScrollView)typeof(ScrollViewRenderer)
                    .GetField("_hScrollView", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
                _scrollView.Touch += HScrollViewTouch;
                _scrollView.ScrollChange += OnScroll;
            }
            if (e.PropertyName == CarouselViewLayout.SelectedIndexProperty.PropertyName && !_motionDown)
            {
                ScrollToIndex(((CarouselViewLayout)this.Element).SelectedIndex);
            }
        }

        private void OnScroll(object sender, global::Android.Views.View.ScrollChangeEventArgs e)
        {
            if (_swipeTimer.Enabled)
            {
                _swipeTimer.Stop();
                _direction = e.ScrollX.CompareTo(e.OldScrollX);
                _swipeTimer.Start();
            }
        }

        void HScrollViewTouch(object sender, TouchEventArgs e)
        {
            e.Handled = false;

            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    _motionDown = true;
                    _swipeTimer.Stop();
                    break;
                case MotionEventActions.Up:
                    _motionDown = false;
                    _swipeTimer.Start();
                    break;
            }
        }

        void SnapScroll()
        {
            if (!_motionDown)
            {
                var current = _scrollView.ScrollX / (double)_scrollView.Width;
                var index = _direction > 0 ? System.Math.Ceiling(current)
                          : _direction < 0 ? System.Math.Floor(current)
                          : System.Math.Round(current);

                ScrollToIndex(Convert.ToInt32(index));
            }
        }

        void ScrollToIndex(int targetIndex)
        {
            LayoutElement.SelectedIndex = System.Math.Min(LayoutElement.Children.Count - 1, System.Math.Max(0, targetIndex));
            
            var targetX = LayoutElement.SelectedIndex * _scrollView.Width;
            _scrollView.Post(new Runnable(() => {
                _scrollView.SmoothScrollTo(targetX, 0);
            }));
        }

        bool _initialized = false;
        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            if (_initialized) return;
            _initialized = true;
            var carouselLayout = (CarouselViewLayout)this.Element;
            _scrollView.ScrollTo(carouselLayout.SelectedIndex * Width, 0);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            if (_initialized && (w != oldw))
            {
                _initialized = false;
            }
            base.OnSizeChanged(w, h, oldw, oldh);
        }
    }
}