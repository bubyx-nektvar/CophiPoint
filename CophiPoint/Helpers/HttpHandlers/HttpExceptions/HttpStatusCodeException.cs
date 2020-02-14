using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CophiPoint.Helpers.HttpHandlers.HttpExceptions
{
    public class HttpStatusCodeException : HttpRequestException
    {
        public HttpStatusCode Status { get; }
        public string Content { get; }

        public HttpStatusCodeException(HttpStatusCode status, string content) : base(GetMessage(status))
        {
            Status = status;
            Content = content;
        }

        private static string GetMessage(HttpStatusCode status)
        {
            return $"Http request failed with status {status.ToString()}";
        }
    }
}
