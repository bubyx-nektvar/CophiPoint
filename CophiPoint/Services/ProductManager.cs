using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public class ProductManager
    {
        private IProductService _restService;

        public ProductManager(IProductService restService)
        {
            _restService = restService;
        }

        public ObservableCollection<ProductViewModel> Products { get; } = new ObservableCollection<ProductViewModel>();

        public async Task Load()
        {
            var items = await _restService.GetProducts();
            Products.Clear();

            foreach (var item in items)
            {
                Products.Add(new ProductViewModel(item));
            }
        }
    }
}
