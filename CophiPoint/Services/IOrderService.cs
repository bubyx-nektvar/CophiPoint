using CophiPoint.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public interface IOrderService
    {
        Task<AccountInfo> GetAccountInfo();
        Task<PurchasedItem> AddPuchase(PurchaseOrder order);
    }

    public interface IProductService
    {
        Task<List<Product>> GetProducts();
    }
}
