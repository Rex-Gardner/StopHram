using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UploadController : Controller
    {
        public async Task<IActionResult> UploadAvatarAsync(string id, IFormFile avatar, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (id == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing(nameof(id));
                return BadRequest(error);
            }

            if (avatar == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing(nameof(avatar));
                return BadRequest(error);
            }

            const int maxFileLength = 1024 * 512;
            var extension = Path.GetExtension(avatar.FileName);
            var path = $"/avatars/{id}{extension}";
            var stream = avatar.OpenReadStream();

            if (avatar.Length > 0 && avatar.Length <= maxFileLength && ImageValidationService.IsImage(stream))
            {
                using (var fileStream = new FileStream($"{hostingEnvironment.WebRootPath}{path}", FileMode.Create))
                {
                    await avatar.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
                }

                var placePatchInfo = new Model.PlacePatchInfo(id, null, path);
                await placeRepository.PatchAsync(placePatchInfo, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var error = ServiceErrorResponses.InvalidImageData(nameof(avatar));
                return BadRequest(error);
            }

            var clientAvatar = new ClientModels.Avatar(path);
            return Ok(clientAvatar);
        }
    }
}
