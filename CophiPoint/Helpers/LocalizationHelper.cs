using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CophiPoint.Helpers
{
    public class LocalizationHelper
    {
        public static CultureInfo BuildLocalization(string netLanguage, Func<PlatformCulture,string> ToDotnetFallbackLanguage)
        {
            // this gets called a lot - try/catch can be expensive so consider caching or something
            CultureInfo ci;
            try
            {
                ci = new CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException)
            {
                // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
                // fall-back to first characters, in this case "en"
                try
                {
                    var fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
                    ci = new CultureInfo(fallback);
                }
                catch (CultureNotFoundException)
                {
                    // iOS language not valid .NET culture, falling back to English
                    ci = new CultureInfo("en");
                }
            }
            return ci;
        }
    }
}
