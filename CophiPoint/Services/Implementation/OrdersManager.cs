using CophiPoint.Api;
using CophiPoint.Api.Models;
using CophiPoint.Extensions;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;

namespace CophiPoint.Services.Implementation
{
    public class OrderManager
    {
        private readonly IOrderService _restService;
        private readonly AuthService _authService;
        private readonly ProductManager _productManager;
        private readonly ICacheService _cache;

        public ObservableCollection<PurchasedItemViewModel> Items { get; } = new ObservableCollection<PurchasedItemViewModel>();
        public UserViewModel Info { get; } = new UserViewModel();

        public OrderManager(IOrderService restService, ProductManager productManager, AuthService authService, ICacheService cache)
        {
            _restService = restService;
            _authService = authService;
            _productManager = productManager;
            _cache = cache;
        }
        public async Task AddItem(ProductViewModel selected)
        {
            var orderItem = new PurchaseOrder
            {
                ProductId = selected._model.Id,
                Size = selected.SelectedSize._size
            };
            
            bool retry = true;
            while (retry)
            {
                retry = false;
                try
                {
                    var addedItem = await _restService.AddPuchase(orderItem);
                    Items.Add(GetViewModel(addedItem));
                    Info.Balance += addedItem.TotalPrice;
                    return;
                }
                catch (DataConflictException ex)
                {
                    await _productManager.Load();

                    if (selected._model.Id == ex.Proposal.ProductId)
                    {
                        var alertMessage = string.Format(GeneralResources.ProductPriceChangedAlert
                            , selected._model.Name
                            , ex.Proposal.Size.UnitsCount.GetSizeString(selected._model.Unit)
                            , ex.Proposal.Size.TotalPrice.GetPriceString());

                        if(await App.Current.MainPage.DisplayAlert(GeneralResources.OrderConfirmAlertTitle, alertMessage, GeneralResources.AlertAdd, GeneralResources.AlertCancel))
                        {
                            retry = true;
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(GeneralResources.OrderFailedTitle, GeneralResources.OrderFailedMsg, GeneralResources.AlertCancel);
                    }
                }
            }
        }

        public async Task Load()
        {
            Console.WriteLine("Loading Account");
            var info = await _restService.GetAccountInfo();
            Info.Set(info);

            Console.WriteLine("Loading Purchases");
            var items = info.Orders;
            Items.Clear();

            foreach (var item in items)
            {
                Items.Add(GetViewModel(item));
            }
            Console.WriteLine("Loading Orders Done");
            
            if(await _cache.SetRequiredVersionToAll(info.DataVersion))
            {
                await TinyIoCContainer.Current.Resolve<ProductManager>().Load();
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
