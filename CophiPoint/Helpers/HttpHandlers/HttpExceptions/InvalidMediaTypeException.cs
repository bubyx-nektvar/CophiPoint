using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CophiPoint.Helpers.HttpHandlers.HttpExceptions
{
    public class InvalidMediaTypeException : HttpRequestException
    {
        public string MediaType { get; }
        public string ExpectedMediaType { get; }

        public InvalidMediaTypeException(string mediaType, string expectedMediaType, string message) : base(message)
        {
            MediaType = mediaType;
            ExpectedMediaType = expectedMediaType;
        }
    }
}
