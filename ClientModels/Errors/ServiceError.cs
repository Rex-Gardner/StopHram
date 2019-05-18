using System;
namespace ClientModels.Errors
{
    public class ServiceError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }

        public ServiceError()
        {
        }
    }
}
