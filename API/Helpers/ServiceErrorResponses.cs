using System;
using ClientModels.Errors;

namespace API.Helpers
{
    public static class ServiceErrorResponses
    {
        public static ErrorResponseService BodyIsMissing(string target)
        {
            return new ErrorResponseService
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = System.Net.HttpStatusCode.BadRequest.ToString(),
                    Message = "Body is Missing!",
                    Target = target
                }
            };
        }
    }
}
