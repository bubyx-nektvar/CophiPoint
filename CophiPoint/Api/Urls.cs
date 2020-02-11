using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CophiPoint.Api
{
    public class Urls
    {
        public class OIDCUrls
        {

            public string Authorization { get; set; }
            public string Token { get; set; }
        }
        public class ShopUrls
        {
            public string Products { get; set; }
            public string User { get; set; }
        }

        public class InfoUrls
        {
            public string General { get; set; }
            public string Payment { get; set; }
        }

        public string Domain { get; set; }

        public OIDCUrls OIDC { get; set; }
        public InfoUrls Info { get; set; }
        public ShopUrls Shop { get; set; }


        public Uri GetBaseAddress() => new Uri(Domain);
        public Uri GetUrl(string relativepath) => new Uri(GetBaseAddress(), relativepath);

        public OIDCUrls GetOIDCFullPathUrls() => GetFullPathUrls(OIDC);
        public InfoUrls GetInfoFullPathUrls() => GetFullPathUrls(Info);

        private T GetFullPathUrls<T>(T value)
            where T:new()
        {
            var r = new T();
            foreach(var prop in typeof(T).GetProperties())
            {
                var relativePath = (string)prop.GetValue(value);
                var absoleuUrl = GetUrl(relativePath).AbsoluteUri;
                prop.SetValue(r, absoleuUrl);
            }
            return r;
        }

    }
}
