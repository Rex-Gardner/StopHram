using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using API.Helpers;
using ClientModels.Errors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model = ClientModels.Picture;

namespace API.Controllers
{
    [ApiController]
    public class UploadHelper
    {
        IHostingEnvironment _hostingEnvironment;

        public UploadHelper(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<ResponseService> UploadPictureAsync(string id, IFormFile picture, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ResponseService response;

            if (id == null)
            {
                response = Responses.BodyIsMissing(nameof(id));
                return response;
            }

            if (picture == null)
            {
                response = Responses.BodyIsMissing(nameof(picture));
                return response;
            }

            const int maxFileLength = 1024 * 512;
            var extension = Path.GetExtension(picture.FileName);
            var path = $"/pictures/{id}{extension}";
            var stream = picture.OpenReadStream();

            if (picture.Length > 0 && picture.Length <= maxFileLength && ImageValidation.IsImage(stream))
            {
                using (var fileStream = new FileStream($"{_hostingEnvironment.WebRootPath}{path}", FileMode.Create))
                {
                    await picture.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                var error = Responses.InvalidImageData(nameof(picture));
                return error;
            }

            response = Responses.Ok(path);
            return response;
        }
    }
}
