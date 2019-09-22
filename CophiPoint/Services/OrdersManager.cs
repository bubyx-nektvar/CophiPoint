using CophiPoint.Api;
using CophiPoint.Api.Models;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public class OrderManager
    {
        private readonly IOrderService _restService;
        private readonly AuthService _authService;

        public ObservableCollection<PurchasedItemViewModel> Items { get; } = new ObservableCollection<PurchasedItemViewModel>();
        public UserViewModel Info { get; } = new UserViewModel();

        public OrderManager(IOrderService restService, AuthService authService)
        {
            _restService = restService;
            _authService = authService;
        }
        public async Task AddItem(ProductViewModel selected)
        {

            try
            {
                var addedItem = await _restService.AddPuchase(new Api.Models.PurchaseOrder
                {
                    ProductId = selected._model.Id,
                    Size = selected.SelectedSize._size
                });
                Items.Add(GetViewModel(addedItem));
                Info.Balance += addedItem.TotalPrice;
            }
            catch (DataConflictException ex)
            {
                await App.Current.MainPage.DisplayAlert("Add order failed", ex.Message, "Cancel");
                //TODO produc conflict
            }
        }

        public async Task Load()
        {
            var info = await _restService.GetAccountInfo();
            Info.Set(info);

            var items = await _restService.GetPurchases();
            Items.Clear();

            foreach (var item in items)
            {
                Items.Add(GetViewModel(item));
            }
        }

        public async Task Logout()
        {
            await _authService.Logout();
            Info.Set(new AccountInfo
            {
                Email = "",
                Balance = 0
            });
            Items.Clear();
        }

        private PurchasedItemViewModel GetViewModel(PurchasedItem item) => new PurchasedItemViewModel(item);
    }
}
