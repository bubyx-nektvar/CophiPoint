using CophiPoint.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CophiPoint.Services;
using CophiPoint.Extensions;

namespace CophiPoint.Api
{
    public class ProductsApi : IProductService
    {
        private readonly IHttpRestService _connectionService;

        public ProductsApi(IHttpRestService urlService)
        {
            _connectionService = urlService;
        }
        
        public async Task<List<Product>> GetProducts()
        {
            return await _connectionService.GetAsync<List<Product>>(urls => urls.Shop.Products);
        }
    }
}
