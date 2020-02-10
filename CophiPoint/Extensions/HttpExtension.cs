using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Extensions
{
    public static class HttpExtension
    {
        public static async Task<TResult> ParseResultOrFail<TResult>(this HttpResponseMessage response)
        {
            using (response)
            {
                response.EnsureSuccessStatusCode();

                return await response.ParseResponseBody<TResult>();
            }
        }

        public static async Task<T> ParseResponseBody<T>(this HttpResponseMessage response)
        {
            if (response.Content.Headers.ContentType.MediaType != "application/json")
                throw new HttpRequestException(GeneralResources.MediaTypeError);

            return await response.ReadAsJson<T>();
        }

        public static async Task<T> ReadAsJson<T>(this HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
