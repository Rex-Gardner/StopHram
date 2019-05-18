using System;
namespace ClientModels.Errors
{
    public class ServiceError
    {
        string Code { get; set; }
        string Message { get; set; }
        string Target { get; set; }

        public ServiceError()
        {
        }
    }
}
