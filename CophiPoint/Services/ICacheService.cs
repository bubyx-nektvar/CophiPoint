using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public interface ICacheService
    {
        Task<bool> IsUpToDate(Uri requestUri);

        Task SetValue(Uri requestUri, string version, string Content, string mediaType);
        
        Task<(string content, string mediaType)> GetValue(Uri requestUri);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVersion"></param>
        /// <returns>ture if for some endpoint required version changed</returns>
        Task<bool> SetRequiredVersionToAll(string newVersion);
    }
}
