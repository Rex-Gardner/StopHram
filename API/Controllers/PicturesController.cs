using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api/v1/pictures")]
    public class PicturesController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public PicturesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddPictures(string id, IFormFile picture, CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(picture.FileName);

            if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
            {
                _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var path = $"{_hostingEnvironment.WebRootPath}/pictures/{id}{extension}";
            var response = await UploadHelper.UploadPictureAsync(id, path, picture, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(path);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
