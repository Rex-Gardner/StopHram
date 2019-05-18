using System;
using System.Net;

namespace ClientModels.Errors
{
    public class ResponseService
    {
        public HttpStatusCode StatusCode { get; set; }
        public ServiceResponse Error { get; set; }

        public ResponseService()
        {
        }
    }
}
