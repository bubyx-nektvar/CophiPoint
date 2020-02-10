using CophiPoint.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CophiPoint.Services;

namespace CophiPoint.Api
{
    public class ProductsApi : IProductService
    {
        private readonly ApiConnectionService _connectionService;

        public ProductsApi(ApiConnectionService urlService)
        {
            _connectionService = urlService;
        }
        
        public async Task<List<Product>> GetProducts()
        {
            var response = await _connectionService.SendAsync(HttpMethod.Get, urls => urls.Shop.Products);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(content);
            }
            return null;
        }
    }
}
