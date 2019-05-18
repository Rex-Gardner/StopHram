using System;
using ClientModels.Errors;

namespace API.Helpers
{
    public static class Responses
    {
        public static Response Ok(string target)
        {
            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Responce = new ResponseDetails
                {
                    Code = System.Net.HttpStatusCode.OK.ToString(),
                    Message = ResponseCodes.Ok,
                    Target = target
                }
            };
        }

        public static Response BodyIsMissing(string target)
        {
            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Responce = new ResponseDetails
                {
                    Code = System.Net.HttpStatusCode.BadRequest.ToString(),
                    Message = ResponseCodes.BadRequest,
                    Target = target
                }
            };
        }

        public static Response InvalidImageData(string target)
        {
            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Responce = new ResponseDetails
                {
                    Code = System.Net.HttpStatusCode.BadRequest.ToString(),
                    Message = ResponseCodes.BadRequest,
                    Target = target
                }
            };
        }
    }
}
