using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Api
{
    public static class AuthConstants
    {
        public const string ClientId = "CophiPointAPI";
        public const string ClientSecret = "d763c6623386636b42608de9856731edbca26885540b4de96cdc8d4d";
        public static string Scope => string.Join(" ",ScopesArray); 
        public static string[] ScopesArray = new[] { "openid","email","balance" };
        public const string RedirectUri = "mff.cophipoint://connect";
        public const int ExpirationPeriodMinutes = 60;
    }
}
