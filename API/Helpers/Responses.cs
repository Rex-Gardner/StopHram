using System;
using ClientModels.Errors;

namespace API.Helpers
{
    public static class Responses
    {
        public static ResponseService Ok(string target)
        {
            return new ResponseService
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Error = new ServiceResponse
                {
                    Code = System.Net.HttpStatusCode.OK.ToString(),
                    Message = ResponseCodes.Ok,
                    Target = target
                }
            };
        }

        public static ResponseService BodyIsMissing(string target)
        {
            return new ResponseService
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Error = new ServiceResponse
                {
                    Code = System.Net.HttpStatusCode.BadRequest.ToString(),
                    Message = ResponseCodes.BadRequest,
                    Target = target
                }
            };
        }

        public static ResponseService InvalidImageData(string target)
        {
            return new ResponseService
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Error = new ServiceResponse
                {
                    Code = System.Net.HttpStatusCode.BadRequest.ToString(),
                    Message = ResponseCodes.BadRequest,
                    Target = target
                }
            };
        }
    }
}
