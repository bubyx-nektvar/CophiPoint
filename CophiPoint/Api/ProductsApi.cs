using CophiPoint.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Api
{
    public class ProductsApi
    {
        private readonly HttpClient _client;

        public ProductsApi()
        {
            _client = new HttpClient();
            _client.BaseAddress = Urls.BaseUrl;
        }
        
        public async Task<List<Product>> GetProducts()
        {
            var response = await _client.GetAsync(Urls.Products);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(content);
            }
            return null;
        }
    }
}
