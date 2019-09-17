using CophiPoint.Api.Models;
using FFImageLoading.Svg.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CophiPoint.ViewModels
{
    public class ProductViewModel: INotifyPropertyChanged
    {
        private Product _model;

        public string Name => _model.Name;

        public Uri ImageUrl => _model.Image;

        public static ProductViewModel Empty => new ProductViewModel(new Product()
        {
            Sizes = new[]
            {
                new Api.Models.Size()
                {
                    Price = 1,
                    UnitsCount = 1
                }
            },
            Unit = Unit.MiliLiters,
            Name = "",
            Image = new Uri("data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNCIgaGVpZ2h0PSIyNCIgdmlld0JveD0iMCAwIDI0IDI0Ij48cGF0aCBkPSJNMCAwaDI0djI0SDB6IiBmaWxsPSJub25lIi8+PHBhdGggZD0iTTIxIDE5VjVjMC0xLjEtLjktMi0yLTJINWMtMS4xIDAtMiAuOS0yIDJ2MTRjMCAxLjEuOSAyIDIgMmgxNGMxLjEgMCAyLS45IDItMnpNOC41IDEzLjVsMi41IDMuMDFMMTQuNSAxMmw0LjUgNkg1bDMuNS00LjV6Ii8+PC9zdmc+")
        });

        public ObservableCollection<SizeViewModel> Sizes { get; set; }
        
        private SizeViewModel _selectedSize;

        public SizeViewModel SelectedSize
        {
            get => _selectedSize;
            set
            {
                if(_selectedSize != value)
                {
                    _selectedSize = value;
                    OnPropertyChanged(nameof(SelectedSize));
                }
            }
        }

        private bool _favorite;
        public bool Favorite {
            get =>_favorite;
            set
            {
                if(value != _favorite)
                {
                    _favorite = value;
                    OnPropertyChanged(nameof(Favorite));
                }
            }
        } //TODO store favorite

        private bool _selectSizeVisible;
        public bool SelectSizeVisible
        {
            get => _selectSizeVisible;
            set
            {
                if(value != _selectSizeVisible)
                {
                    _selectSizeVisible = value;
                    OnPropertyChanged(nameof(SelectSizeVisible));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ProductViewModel(Product product)
        {
            _model = product;
            var unitImage = product.Unit.ImageSource();
            Sizes = new ObservableCollection<SizeViewModel>(product.Sizes.Select(x => new SizeViewModel(x, product.Unit, unitImage)));
            SelectedSize = Sizes.First();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
