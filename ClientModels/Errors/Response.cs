using System;
using System.Net;

namespace ClientModels.Errors
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public ResponseDetails ResponseDetails { get; set; }

        public Response()
        {
        }
    }
}
