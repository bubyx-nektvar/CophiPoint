using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Api
{
    public static class AuthConstants
    {
        public const string ClientId = "TODO";//TODO customize
        public const string ClientSecret = "TODO";//TODO customize
        public const string RedirectUri = "mff.cophipoint://connect";//TODO customize

        public static string Scope => string.Join(" ",ScopesArray); 
        public static string[] ScopesArray = new[] { "openid","email","balance" };
        public const int ExpirationPeriodMinutes = 60;
    }
}
