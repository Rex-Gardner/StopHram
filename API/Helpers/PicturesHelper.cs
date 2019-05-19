using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using API.Helpers;
using ClientModels;
using ClientModels.Errors;
using ClientModels.Pictures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace API.Helpers
{
    public static class PicturesHelper
    {
        private const int DefaultImageId = 0;

        public const string PicturesDirName = "pictures";

        public static Response ToResponse(this PictureDeleteResult deleteResult)
        {
            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ResponseDetails = new ResponseDetails
                {
                    Code = ResponseCodes.BadRequest,
                    Message = deleteResult.Message,
                    Target = deleteResult.Picture.Url
                }
            };
        }

        public static string GetPathFromEnvironment(IHostingEnvironment hostingEnvironment)
        {
            if (string.IsNullOrWhiteSpace(hostingEnvironment.WebRootPath))
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            return hostingEnvironment.WebRootPath;
        }

        public static string GetDirName(IHostingEnvironment hostingEnvironment, string troubleId)
        {
            return $"{GetPathFromEnvironment(hostingEnvironment)}/{PicturesDirName}/{troubleId}";
        }

        public static string[] GetImageUrlsForTrouble(IHostingEnvironment hostingEnvironment, string troubleId)
        {
            var dir = GetDirName(hostingEnvironment, troubleId);

            return Directory.GetFiles(dir);
        }

        public static PictureDeleteResult DeleteFile(string fileName)
        {
            var file = new FileInfo(fileName);

            if (file.Exists)
            {
                try
                {
                    file.Delete();

                    return new PictureDeleteResult
                    {
                        Code = DeleteCode.Success,
                        Message = "Ok",
                        Picture = new Picture(file.FullName)
                    };
                }
                catch (Exception e)
                {
                    return new PictureDeleteResult
                    {
                        Code = DeleteCode.Error,
                        Message = e.Message,
                        Picture = new Picture(file.FullName)
                    };
                }
            }

            return new PictureDeleteResult
            {
                Code = DeleteCode.Error,
                Message = "File not Exists on server",
                Picture = new Picture(file.FullName)
            };
        }

        public static List<PictureDeleteResult> DeleteAllPicturesForTrouble(IHostingEnvironment hostingEnvironment, string troubleId)
        {
            var deleted = new List<PictureDeleteResult>();
            var dir = GetDirName(hostingEnvironment, troubleId);

            if (Directory.Exists(dir))
            {
                var directoryInfo = new DirectoryInfo(dir);

                var files = directoryInfo.GetFiles();
                foreach (var file in files)
                {
                    try
                    {
                        file.Delete();

                        deleted.Add(new PictureDeleteResult
                        {
                            Code = DeleteCode.Success,
                            Message = "Ok",
                            Picture = new Picture(file.FullName)
                        });
                    }
                    catch (Exception e)
                    {
                        deleted.Add(new PictureDeleteResult
                        {
                            Code = DeleteCode.Error,
                            Message = e.Message,
                            Picture = new Picture(file.FullName)
                        });
                    }
                }
            }

            return deleted;
        }

        public static async Task<Response> UploadPictureAsync(IHostingEnvironment hostingEnvironment, 
                                                              string path, 
                                                              IFormFile picture, 
                                                              CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Response response;

            if (picture == null)
            {
                response = Responses.BodyIsMissing(nameof(picture));
                return response;
            }

            const int maxFileLength = 1024 * 512;
            var stream = picture.OpenReadStream();

            if (picture.Length > 0 && picture.Length <= maxFileLength && ImageValidation.IsImage(stream))
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await picture.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                var error = Responses.InvalidImageData(nameof(picture));
                return error;
            }

            response = Responses.Ok(path, "Picture");
            return response;
        }

        public static int GetAvailableIdForPicture(string path)
        {
            string[] filePaths = Directory.GetFiles(path);

            if (filePaths.Length == 0)
            {
                return DefaultImageId;
            }

            var id = DefaultImageId;
            foreach (var filePath in filePaths)
            {
                var filename = Path.GetFileNameWithoutExtension(filePath);

                if (int.TryParse(filename, out var currentId))
                {
                    if (currentId > id)
                    {
                        id = currentId;
                    }
                }
            }

            return id + 1;
        }
    }
}
