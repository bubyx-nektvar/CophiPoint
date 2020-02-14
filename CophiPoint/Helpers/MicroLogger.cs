using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Helpers
{
    public class MicroLogger
    {
        public static void LogDebug(string message)
        {
#if DEBUG
            Console.WriteLine("DEBUG: {0}", message);
#endif
        }
        public static void LogWarning(string message)
        {
            Console.WriteLine("WARN: {0}",message);
        }
        public static void LogError(string message)
        {
            Console.WriteLine("ERROR: {0}",message);
        }
    }
}
