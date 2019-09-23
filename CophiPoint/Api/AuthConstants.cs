using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Api
{
    public static class AuthConstants
    {
        public const string ClientId = "4edRsLK6ORhq";
        public const string ClientSecret = "d763c6623386636b42608de9856731edbca26885540b4de96cdc8d4d";
        public static string Scope => string.Join(" ",ScopesArray); 
        public static string[] ScopesArray = new[] { "openid","profile","email" };/*add offline_access if using MojeId Pro version*/
        public const string RedirectUri = "https://cophipoint.bubyx.cz/connect.php";
        public const string ConfigUrl = "https://mojeid.regtest.nic.cz/.well-known/openid-configuration/";
        public const int ExpirationPeriodMinutes = 60;
    }
}
