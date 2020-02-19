using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Api
{
    public static class AuthConstants
    {
        public const string ClientId = "$CLIENT_ID";//TODO customize
        public const string ClientSecret = "$CLIENT_SECRET";//TODO customize
        public const string RedirectUri = "$CUSTOM_SCHEME://connect";//TODO customize

        public static string Scope => string.Join(" ",ScopesArray); 
        public static string[] ScopesArray = new[] { "openid","email","balance" };
        public const int ExpirationPeriodMinutes = 60;
    }
}
