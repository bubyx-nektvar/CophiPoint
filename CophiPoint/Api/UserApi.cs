using CophiPoint.Api.Models;
using CophiPoint.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Api
{
    public class UserApi : IOrderService
    {
        private readonly AuthService _auth;
        private readonly HttpClient _client;

        public UserApi(AuthService auth)
        {
            _auth = auth;
            _client = new HttpClient();
            _client.BaseAddress = Urls.BaseUrl;
        }
        
        public async Task<List<PurchasedItem>> GetPurchases()
        {
            _client.DefaultRequestHeaders.Authorization = await _auth.GetAccessToken();
            var response = await _client.GetAsync(Urls.UserOrdersApi);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PurchasedItem>>(content);
            }
            return null;
        }

        public async Task<AccountInfo> GetAccountInfo()
        {
            _client.DefaultRequestHeaders.Authorization = await _auth.GetAccessToken();
            var response = await _client.GetAsync(Urls.UserApi);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AccountInfo>(result);
            }
            //TODO nedostupne
            return null;
        }
        public async Task<PurchasedItem> AddPuchase(PurchaseOrder purchase)
        {
            _client.DefaultRequestHeaders.Authorization = await _auth.GetAccessToken();

            var json = JsonConvert.SerializeObject(purchase);
            var content = new StringContent(json,Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(Urls.UserOrdersApi, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PurchasedItem>(result);
            }
            //TODO badrequests etc.
            return null;
        }
    }
}
