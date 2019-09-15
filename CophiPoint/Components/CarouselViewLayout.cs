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
    public class CarouselViewLayout : ScrollView
    {
        private readonly StackLayout _stack;
        
        private bool _layingOutChildren;


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
        public int SelectedIndex { get; set; }

        public new IList<View> Children => _stack.Children;

        public CarouselViewLayout()
        {
            Orientation = ScrollOrientation.Horizontal;
            _stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0
            };
            Content = _stack;
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
            if (_layingOutChildren) return;

            _layingOutChildren = true;
            foreach (var child in Children) child.WidthRequest = width;
            _layingOutChildren = false;

        }

        void ItemsSourceChanging()
        {
            if (ItemsSource == null) return;
            SelectedIndex = 0;
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