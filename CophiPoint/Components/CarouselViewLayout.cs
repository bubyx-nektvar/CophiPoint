using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CophiPoint.Components
{
    public class CarouselViewLayout : ContentView
    {
        private readonly StackLayout _stack;
        private readonly ICommand _swipeCommand;

        private bool _layingOutChildren;

        private int _selectedIndex;

        //public IList<View> Children => _scrollView._stack.Children;


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(CarouselViewLayout),
                null,
                propertyChanging: (bindableObject, oldValue, newValue) =>
                {
                    ((CarouselViewLayout)bindableObject).ItemsSourceChanging();
                },
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {
                    ((CarouselViewLayout)bindableObject).ItemsSourceChanged();
                }
            );

        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public DataTemplate ItemTemplate { get; set; }
        public new IList<View> Children => _stack.Children;

        public CarouselViewLayout()
        {

            _swipeCommand = new Command<int>(Swipe);
            _stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0
            };
            Content = _stack;
            GestureRecognizers.Add(new SwipeGestureRecognizer()
            {
                Direction = SwipeDirection.Left,
                Command = _swipeCommand,
                CommandParameter = 1
            });
            GestureRecognizers.Add(new SwipeGestureRecognizer()
            {
                Direction = SwipeDirection.Right,
                Command = _swipeCommand,
                CommandParameter = -1,
            });
            var x = new PanGestureRecognizer();
            x.PanUpdated += pan;
            GestureRecognizers.Add(x);
        }
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
            if (_layingOutChildren) return;

            _layingOutChildren = true;
            foreach (var child in Children) child.WidthRequest = width;
            _layingOutChildren = false;

        }

        private async void pan(object sender, PanUpdatedEventArgs e)
        {
            if(e.StatusType == GestureStatus.Completed)
            {
                await SwipeToPosition(-1 * Convert.ToInt32(Math.Round(_stack.TranslationX / Width)));
            }
            else if (e.StatusType == GestureStatus.Running)
            {
                _stack.TranslationX = (-_selectedIndex * Width) + e.TotalX;
            }
        }

        private async void Swipe(int difference) => await SwipeToPosition(_selectedIndex + difference);
        
        private async Task SwipeToPosition(int position)
        {
            _selectedIndex = Math.Min(Children.Count - 1, Math.Max(0, position));
            await _stack.TranslateTo(-_selectedIndex * Width, 0);
        }


        void ItemsSourceChanging()
        {
            if (ItemsSource == null) return;
            _selectedIndex = 0;
        }

        void ItemsSourceChanged()
        {
            _stack.Children.Clear();
            foreach (var item in ItemsSource)
            {
                var view = (View)ItemTemplate.CreateContent();
                var bindableObject = view as BindableObject;
                if (bindableObject != null)
                    bindableObject.BindingContext = item;
                _stack.Children.Add(view);
            }
        }
    }
}