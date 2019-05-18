using System;
using ClientModels.Errors;

namespace API.Helpers
{
    public static class Responses
    {
        public static ErrorResponseService Ok(string target)
        {
            return new ErrorResponseService
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Error = new ServiceError
                {
                    Code = System.Net.HttpStatusCode.OK.ToString(),
                    Message = "OK",
                    Target = target
                }
            };
        }

        public static ErrorResponseService BodyIsMissing(string target)
        {
            return new ErrorResponseService
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = System.Net.HttpStatusCode.BadRequest.ToString(),
                    Message = "Body is Missing",
                    Target = target
                }
            };
        }

        public static ErrorResponseService InvalidImageData(string target)
        {
            return new ErrorResponseService
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = System.Net.HttpStatusCode.BadRequest.ToString(),
                    Message = "Invalid Image Data",
                    Target = target
                }
            };
        }
    }
}
