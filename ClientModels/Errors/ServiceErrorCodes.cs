using System;
namespace ClientModels.Errors
{
    public static class ServiceErrorCodes
    {
        public const string UnsupportedMediaType = "Unsupported Media Type";
        public const string ServiceUnavailable = "Service Unavailable";
        public const string InternalServerError = "Internal Server Error";
        public const string Unauthorized = "Unauthorized";
        public const string Forbidden = "Forbidden";
        public const string NotFound = "Not Found";
    }
}
