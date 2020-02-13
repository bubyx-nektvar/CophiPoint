using CophiPoint.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public interface IHttpRestService
    {
        Task<HttpResponseMessage> PostAuthorizedAsync<T>(T contentObject, Func<Urls, string> relativePathSelector);
        
        Task<TResponse> GetAuthorizedAsync<TResponse>(Func<Urls, string> relativePathSelector, bool cache = true);
        
        Task<TResponse> GetJsonAsync<TResponse>(Func<Urls, string> relativePathSelector);
        
        Task<string> GetHtmlAsync(Func<Urls, string> relativePathSelector);

        Task<Urls> GetUrls();
    }
}
