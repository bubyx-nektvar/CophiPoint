using CophiPoint.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// Get information about user purchuases and account summary.
        /// </summary>
        /// <returns></returns>
        Task<AccountInfo> GetAccountInfo();

        /// <summary>
        /// Send order request to server.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Server purchuase entry</returns>
        /// <exception cref="DataConflictException">When product definition has changed on server</exception>
        Task<PurchasedItem> AddPuchase(PurchaseOrder order);
    }

    public interface IProductService
    {
        /// <summary>
        /// Get available products and sizes with prices.
        /// </summary>
        /// <returns>Available products</returns>
        Task<List<Product>> GetProducts();
    }
}
