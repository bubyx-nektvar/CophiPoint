using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public class CachedItem
    {
        public string Content { get; set; }
        public string MediaType { get; set; }
        public string Version { get; set; }
    }

    public interface ICacheService
    {
        string Version { get; }
        CachedItem Get(Uri key);
        Task Set(Uri key, CachedItem value);
        Task Clear();
        bool Contains(Uri requestUri);
    }
}
