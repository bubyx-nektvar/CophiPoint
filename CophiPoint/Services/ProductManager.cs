using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Linq;

namespace CophiPoint.Services
{
    public class ProductManager
    {
        private readonly IProductService _restService;
        private ProductViewModel _favorite;

        public ProductManager(IProductService restService)
        {
            _restService = restService;
        }

        public ObservableCollection<ProductViewModel> Products { get; } = new ObservableCollection<ProductViewModel>();

        public ProductViewModel Favorite => _favorite ?? Products.FirstOrDefault() ?? ProductViewModel.Empty;

        public async Task Load()
        {
            var items = await _restService.GetProducts();
            Products.Clear();
            var favorite = Preferences.Get(nameof(ProductViewModel.Favorite), 0, nameof(ProductManager));

            foreach (var item in items)
            {
                var model = new ProductViewModel(item);
                Products.Add(model);
                
                if(item.Id == favorite)
                {
                    model.Favorite = true;
                    _favorite = model;
                }
            }
        }

        public void SetFavorite(ProductViewModel product, bool favorite)
        {
            if (favorite)
            {
                if (Preferences.ContainsKey(nameof(product.Favorite), nameof(ProductManager)))
                {
                    var origin = Preferences.Get(nameof(product.Favorite), 0, nameof(ProductManager));
                    foreach(var p in Products.Where(x => x.Favorite)){
                        p.Favorite =false;
                    }
                }
                Preferences.Set(nameof(product.Favorite), product._model.Id, nameof(ProductManager));
                _favorite = product;
            }
            else
            {
                Preferences.Remove(nameof(product.Favorite), nameof(ProductManager));
                _favorite = null;
            }
            product.Favorite = favorite;
        }
    }
}
