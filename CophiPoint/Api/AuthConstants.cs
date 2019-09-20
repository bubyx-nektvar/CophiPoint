using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Api
{
    public static class AuthConstants
    {
        public const string AuthorityUri = "https://mojeid.regtest.nic.cz/oidc/";
        public const string ClientId = "Vlq9CwolBv7O";
        public const string Scope = "openid profile email";
        public const string RedirectUri = "io.identitymodel.native://callback";
    }
}
