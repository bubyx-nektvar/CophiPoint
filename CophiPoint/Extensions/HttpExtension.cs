using CophiPoint.Helpers;
using CophiPoint.Helpers.HttpHandlers.HttpExceptions;
using CophiPoint.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint.Extensions
{
    public static class HttpExtension
    {
        public const string JsonMediaType = "application/json";
        public const string HtmlMediaType = "text/html";
        private const int MaxRetries = 3;

        public static Task<T> ParseJsonResponseBody<T>(this HttpResponseMessage response)
            => ParseXResponseBody(response, JsonMediaType, JsonConvert.DeserializeObject<T>);

        public static Task<string> ParseHtmlResponseBody(this HttpResponseMessage response)
            => ParseXResponseBody(response, HtmlMediaType, x => x);

        private static async Task<T> ParseXResponseBody<T>(HttpResponseMessage response, string requiredMediaType, Func<string, T> parse)
        {

            using (response)
            {
                if (response.Content.Headers.ContentType.MediaType != requiredMediaType)
                    throw new InvalidMediaTypeException(response.Content.Headers.ContentType.MediaType, requiredMediaType, GeneralResources.MediaTypeError);

                var result = await response.Content.ReadAsStringAsync();
                return parse(result);
            }
        }
        
        public static Task<TResult> AskRetryOnHttpStatusFail<TResult>(Func<Task<TResult>> requestFunction)
            where TResult : new() 
            => AskRetryOnHttpStatusFail(requestFunction, 0);


        private static async Task<TResult> AskRetryOnHttpStatusFail<TResult>(Func<Task<TResult>> requestFunction, int retryCounter)
            where TResult:new()
        {
            try
            {
                return await requestFunction();
            }
            catch (HttpStatusCodeException ex)
            {
                MicroLogger.LogError(ex.Message);

                if (retryCounter < MaxRetries)
                {
                    await App.Current.MainPage.DisplayAlert(GeneralResources.ServerRequestFailedTitle, GeneralResources.ServerRequestFailedRetryMsg, GeneralResources.AlertRetry);

                    return await AskRetryOnHttpStatusFail(requestFunction, retryCounter + 1);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(GeneralResources.ServerRequestFailedTitle, GeneralResources.ServerRequestFailedRetryMsg, GeneralResources.AlertExit);
                    DependencyService.Get<INativeAppService>().ExitApp();
                    return new TResult();
                }
            }
        }
    }
}
