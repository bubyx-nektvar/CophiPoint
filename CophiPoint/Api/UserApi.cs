using CophiPoint.Api.Models;
using CophiPoint.Extensions;
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
        private readonly IHttpRestService _connectionService;

        public UserApi(IHttpRestService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<AccountInfo> GetAccountInfo()
            => await _connectionService.GetAuthorizedAsync<AccountInfo>(u => u.Shop.User, cache: false);

        public async Task<PurchasedItem> AddPuchase(PurchaseOrder purchase)
        {
            using (var result = await _connectionService.PostAuthorizedAsync(purchase, u => u.Shop.User))
            {
                if (result.IsSuccessStatusCode)
                {
                    return await result.ParseResponseBody<PurchasedItem>();
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    var proposal = await result.ReadAsJson<PurchaseOrder>();
                    throw new DataConflictException(proposal);
                }
                else
                {
                    throw new HttpRequestException(GeneralResources.StatusCodeError);
                }
            }

        }
    }
}
