using System;
using System.Net;
using ClientModels.Errors;

namespace API.Helpers
{
    public static class Responses
    {
        public static Response Ok(string message, string target)
        {
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                ResponseDetails = new ResponseDetails
                {
                    Code = ResponseCodes.Ok,
                    Message = message,
                    Target = target
                }
            };
        }

        public static Response BodyIsMissing(string target)
        {
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                ResponseDetails = new ResponseDetails
                {
                    Code =  ResponseCodes.BadRequest,
                    Message = "Request body is empty.",
                    Target = target
                }
            };
        }

        public static Response DuplicationError(string message, string target)
        {
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                ResponseDetails = new ResponseDetails
                {
                    Code =  ResponseCodes.BadRequest,
                    Message = message,
                    Target = target
                }
            };
        }

        public static Response NotFoundError(string message, string target)
        {
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                ResponseDetails = new ResponseDetails
                {
                    Code =  ResponseCodes.NotFound,
                    Message = message,
                    Target = target
                }
            };
        }
        
        public static Response InvalidData(string message, string target)
        {
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                ResponseDetails = new ResponseDetails
                {
                    Code = ResponseCodes.BadRequest,
                    Message = message,
                    Target = target
                }
            };
        }

        public static Response InvalidImageData(string target)
        {
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                ResponseDetails = new ResponseDetails
                {
                    Code = ResponseCodes.BadRequest,
                    Message = "Uploaded image is invalid.",
                    Target = target
                }
            };
        }

        public static Response InvalidId(string message, string target)
        {
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                ResponseDetails = new ResponseDetails
                {
                    Code = ResponseCodes.BadRequest,
                    Message = message,
                    Target = target
                }
            };
        }
    }
}
