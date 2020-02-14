using CophiPoint.Api.Models;
using CophiPoint.Extensions;
using CophiPoint.Helpers.HttpHandlers.HttpExceptions;
using CophiPoint.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
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
        { 
            return await HttpExtension.AskRetryOnHttpStatusFail(
                () => _connectionService.GetJsonAuthorizedAsync<AccountInfo>(u => u.Shop.User, cache: false)
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        /// <exception cref="DataConflictException">When product definition has changed on server</exception>
        public async Task<PurchasedItem> AddPuchase(PurchaseOrder purchase)
        {
            return await HttpExtension.AskRetryOnHttpStatusFail(
                async () => {
                    try
                    {
                        return await _connectionService.PostJsonAuthorizedAsync<PurchaseOrder, PurchasedItem>(purchase, u => u.Shop.User);
                    }
                    catch (HttpStatusCodeException ex) when (ex.Status == HttpStatusCode.Conflict)
                    {
                        var proposal = JsonConvert.DeserializeObject<PurchaseOrder>(ex.Content);
                        throw new DataConflictException(proposal);
                    }
                });
        }
    }
}
