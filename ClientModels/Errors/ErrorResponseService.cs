using System;
using System.Net;

namespace ClientModels.Errors
{
    public class ErrorResponseService
    {
        public HttpStatusCode StatusCode { get; set; }
        public ServiceError Error { get; set; }

        public ErrorResponseService()
        {
        }
    }
}
