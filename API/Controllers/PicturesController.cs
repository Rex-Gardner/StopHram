using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ClientModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Troubles.Repositories;

namespace API.Controllers
{
    /// <summary>
    /// Pictures controller.
    /// </summary>
    [Route("/api/v1/pictures")]
    public class PicturesController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private ITroubleRepository _troubleRepository;

        private void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:API.Controllers.PicturesController"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">Hosting environment.</param>
        public PicturesController(IHostingEnvironment hostingEnvironment, ITroubleRepository troubleRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _troubleRepository = troubleRepository;
        }
        /// <summary>
        /// Добавляет одно изображение на сервер.
        /// </summary>
        /// <returns>Объект Picture.</returns>
        /// <param name="troubleId">Identifier.</param>
        /// <param name="picture">Picture.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpPost("UploadPicture")]
        public async Task<IActionResult> AddPicture(string troubleId, IFormFile picture, CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(picture.FileName);

            if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
            {
                _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var id = 0;
            CreateDirectoryIfNotExists($"{_hostingEnvironment.WebRootPath}/pictures/{troubleId}");
            var path = $"{_hostingEnvironment.WebRootPath}/pictures/{troubleId}/{id}{extension}";
            var response = await UploadHelper.UploadPictureAsync(troubleId, path, picture, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var troublePatchInfo = new TroublePatchInfo(id, null, path);
                await _troubleRepository.PatchAsync(troublePatchInfo, cancellationToken).ConfigureAwait(false);

                return Ok(new Picture(path));
            }
            else
            {
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Добавляет несколько изображений на сервер.
        /// </summary>
        /// <returns>Лист с объектами типа Picture.</returns>
        /// <param name="pictures">Коллекция файлов</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPost("UploadPictures")]
        public async Task<IActionResult> AddPictures(string troubleId, List<IFormFile> pictures, CancellationToken cancellationToken)
        {
            var result = new List<Picture>();

            int id = 0;
            foreach (var picture in pictures)
            {
                var extension = Path.GetExtension(picture.FileName);

                if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
                {
                    _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                CreateDirectoryIfNotExists($"{_hostingEnvironment.WebRootPath}/pictures/{troubleId}");
                var path = $"{_hostingEnvironment.WebRootPath}/pictures/{troubleId}/{id}{extension}";
                var response = await UploadHelper.UploadPictureAsync(id.ToString(), path, picture, cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var troublePatchInfo = new TroublePatchInfo(id, null, path);
                    await _troubleRepository.PatchAsync(troublePatchInfo, cancellationToken).ConfigureAwait(false);

                    result.Add(new Picture(path));
                }
                else
                {
                    return BadRequest(response);
                }

                id++;
            }

            return Ok(result);
        }
    }
}
