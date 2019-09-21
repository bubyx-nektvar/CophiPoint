using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Api
{
    public static class AuthConstants
    {
        public const string AuthorityUri = "https://mojeid.regtest.nic.cz/oidc/";
        public const string ClientId = "4edRsLK6ORhq";
        public const string ClientSecret = "d763c6623386636b42608de9856731edbca26885540b4de96cdc8d4d";
        public const string Scope = "openid profile email";
        public const string RedirectUri = "https://cophipoint.bubyx.cz/connect.php";
        public const string ConfigUrl = "https://mojeid.regtest.nic.cz/.well-known/openid-configuration/";
    }
}
