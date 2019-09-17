using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CophiPoint.Api
{
    public static class Urls
    {
        private const string Domain = "https://bubyx.cz";
        
        public static Uri BaseUrl => new Uri(Domain);

        public const string InfoPage = Domain +"/info";
        public const string PaymentPage = Domain + "/info/payment";
        public const string UserOrdersApi = UserApi + "/orders";
        public const string Products = "products";
        public const string UserApi = "account";
    }
}
