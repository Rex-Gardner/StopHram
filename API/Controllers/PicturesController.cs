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
using Models.Troubles;
using Models.Troubles.Repositories;
using ModelConverters.Troubles;
using System.Linq;
using ClientModels.Errors;
using static API.Helpers.PicturesHelper;
using ClientModels.Pictures;

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
        /// Добавляет одно изображение на сервер. Если уже есть изображения, добавляет к ним
        /// </summary>
        /// <returns>Объект Picture.</returns>
        /// <param name="troubleId">Identifier.</param>
        /// <param name="picture">Picture.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpPost("UploadPicture")]
        public async Task<IActionResult> AddPicture(string troubleId, IFormFile picture, CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(picture.FileName);

            var dir = GetDirName(_hostingEnvironment, troubleId);
            CreateDirectoryIfNotExists(dir);
            var path = $"{dir}/{GetAvailableIdForPicture(dir)}{extension}";

            var response = await UploadPictureAsync(_hostingEnvironment, path, picture, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var paths = GetImageUrlsForTrouble(_hostingEnvironment, troubleId);
                var troublePatchInfo = new TroublePatchInfo(TroubleConverterUtils.ConvertId(troubleId), null, null, paths,
                    null, null, null, null, null);

                await _troubleRepository.PatchAsync(troublePatchInfo, cancellationToken).ConfigureAwait(false);

                return Ok(new Picture(path));
            }
            else
            {
                return BadRequest(response);
            }
        }

        /// Добавляет несколько изображений на сервер. Если уже есть изображения, добавляет к ним
        /// <summary>
        /// </summary>
        /// <returns>Лист с объектами типа Picture.</returns>
        /// <param name="pictures">Коллекция файлов</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPost("UploadPictures")]
        public async Task<IActionResult> AddPictures(string troubleId, List<IFormFile> pictures, CancellationToken cancellationToken)
        {
            var result = new List<Picture>();

            var dir = GetDirName(_hostingEnvironment, troubleId);
            CreateDirectoryIfNotExists(dir);
            int id = GetAvailableIdForPicture(dir);
            foreach (var picture in pictures)
            {
                var extension = Path.GetExtension(picture.FileName);

                var path = $"{dir}/{id}{extension}";
                var response = await UploadPictureAsync(_hostingEnvironment, path, picture, cancellationToken);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return BadRequest(response);
                }

                id++;
            }

            var paths = GetImageUrlsForTrouble(_hostingEnvironment, troubleId);
            var troublePatchInfo = new TroublePatchInfo(TroubleConverterUtils.ConvertId(troubleId), null, null, paths,
                null, null, null, null, null);
            await _troubleRepository.PatchAsync(troublePatchInfo, cancellationToken).ConfigureAwait(false);

            foreach (var path in paths)
            {
                result.Add(new Picture(path));
            }

            return Ok(result);
        }

        /// <summary>
        /// Удаляет все картинки для данной trouble
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="troubleId">Trouble identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> DeleteAllPictures([FromQuery] string troubleId, CancellationToken cancellationToken)
        {
            var deleteResults = DeleteAllPicturesForTrouble(_hostingEnvironment, troubleId);

            var paths = new string[] { };

            var troublePatchInfo = new TroublePatchInfo(TroubleConverterUtils.ConvertId(troubleId), null, null, paths,
                null, null, null, null, null);
            await _troubleRepository.PatchAsync(troublePatchInfo, cancellationToken).ConfigureAwait(false);

            foreach(var deleteResult in deleteResults)
            {
                if (deleteResult.Code != DeleteCode.Success)
                {
                    return BadRequest(deleteResult.ToResponse());
                }
            }

            return Ok(deleteResults);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeletePicture([FromQuery] string troubleId, [FromQuery] string pictureUrl, CancellationToken cancellationToken)
        {
            if (!pictureUrl.Contains(troubleId))
            {
                var response = new Response
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ResponseDetails = new ResponseDetails
                    {
                        Code = ResponseCodes.NotFound,
                        Message = "There is no such image URL for given trouble ID",
                        Target = nameof(pictureUrl)
                    }
                };

                return NotFound(response);
            }

            var deleteResult = DeleteFile(pictureUrl);
            if (deleteResult.Code != DeleteCode.Success)
            {
                return BadRequest(deleteResult.ToResponse());
            }

            var paths = GetImageUrlsForTrouble(_hostingEnvironment, troubleId);

            var troublePatchInfo = new TroublePatchInfo(TroubleConverterUtils.ConvertId(troubleId), null, null, paths,
                null, null, null, null, null);
            await _troubleRepository.PatchAsync(troublePatchInfo, cancellationToken).ConfigureAwait(false);

            return Ok(deleteResult);
        }
    }
}
