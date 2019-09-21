using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CophiPoint.Api
{
    public static class Urls
    {
        private const string Domain = "https://cophipoint.bubyx.cz/";
        
        public static Uri BaseUrl => new Uri(Domain);

        public const string InfoPage = Domain +"api/info/info.html";
        public const string PaymentPage = Domain + "api/info/payment.html";
        public const string UserOrdersApi = "api/account/orders.php";
        public const string Products = "api/products.json";
        public const string UserApi = "api/account.php";
    }
}
