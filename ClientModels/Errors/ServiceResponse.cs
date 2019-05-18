using System;
namespace ClientModels.Errors
{
    public class ServiceResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }

        public ServiceResponse()
        {
        }
    }
}
