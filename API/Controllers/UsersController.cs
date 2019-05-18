using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client = ClientModels.Users;
using Model = Models.Users;

namespace API.Controllers
{
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public UsersController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] Client.UserCreationInfo clientCreationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var err = HttpContext;
            if (clientCreationInfo == null)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.BodyIsMissing(nameof(clientUserInfo));
                //return BadRequest(error);
            }

            var modelCreationInfo = ModelConverters.Users.UserCreationInfoConverter.Convert(clientCreationInfo);

            var user = await userManager.FindByNameAsync(modelCreationInfo.UserName);
            if (user != null)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.UserNameAlreadyUse(clientUserInfo.UserName);
                //return BadRequest(error);
            }

            //
            var dateTime = DateTime.UtcNow;

            var modelUser = new User
            {
                UserName = modelCreationInfo.UserName,
                Email = modelCreationInfo.Email,
                PhoneNumber = modelCreationInfo.PhoneNumber,
                CreatedAt = dateTime,
                LastUpdatedAt = dateTime
            };
            var result = await userManager.CreateAsync(modelUser, modelCreationInfo.Password);
            if (!result.Succeeded)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.ValidationError(result.Errors.First().ToString());
                //return BadRequest(modelUser);
            }

            await userManager.AddToRoleAsync(modelUser, "user");
            var clientUser = ModelConverters.Users.UserConverter.Convert(modelUser);
            return CreatedAtRoute("GetUserRoute", new { userName = clientUser.UserName }, clientUser);
        }

        [HttpGet]
        [Route("")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var users = userManager.Users;
            IReadOnlyList<Client.User> clientUsers;
            try
            {
                clientUsers = users.Select(item => ModelConverters.Users.UserConverter.Convert(item)).ToImmutableList();
            }
            catch (ArgumentNullException)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.UserNotFound("users");
                //return NotFound(error);
            }

            return Ok(clientUsers);
        }

        [HttpGet]
        [Route("{userName}", Name = "GetUserRoute")]
        //[Authorize]
        public async Task<IActionResult> GetUser([FromRoute]string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.UserNotFound(id);
                //return BadRequest(error);
            }

            /*var name = HttpContext.User.Identity.Name;

            if (user.UserName.CompareTo(name) != 0)
            {
                return Forbid();
            }*/

            var clientUser = ModelConverters.Users.UserConverter.Convert(user);
            return Ok(clientUser);
        }

        [HttpPatch]
        [Route("{userName}")]
        public async Task<IActionResult> PatchUserAsync([FromRoute] string userName, [FromBody] Client.UserPatchInfo clientPatchInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userName == null)
            {
                throw new NotImplementedException();
            }

            if (clientPatchInfo == null)
            {
                throw new NotImplementedException();
                // var error = ServiceErrorResponses.BodyIsMissing(nameof(clientPatchInfo));
                //return BadRequest(error);
            }

            var modelPatchInfo = ModelConverters.Users.UserPatchInfoConverter.Convert(userName, clientPatchInfo);

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new NotImplementedException();
            }

            var updated = false;

            if (modelPatchInfo.Password != null && modelPatchInfo.OldPassword != null)
            {
                var passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                var passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
                IdentityResult result = await passwordValidator.ValidateAsync(userManager, user, modelPatchInfo.Password);

                if (result.Succeeded)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, modelPatchInfo.Password);
                    updated = true;
                }
            }

            if (modelPatchInfo.CreatedTroubles != null)
            {
                user.CreatedTroubles = modelPatchInfo.CreatedTroubles;
                updated = true;
            }

            if (modelPatchInfo.LikedTroubles != null)
            {
                user.LikedTroubles = modelPatchInfo.LikedTroubles;
                updated = true;
            }

            if (updated)
            {
                await userManager.UpdateAsync(user);
            }

            return Ok();
        }

        [HttpDelete]
        //[Authorize]
        [Route("{userName}")]
        public async Task<ActionResult> Delete([FromRoute]string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userName == null)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.ValidationError("UserId");
                //return BadRequest(error);
            }

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.UserNotFound(id);
                //return NotFound(error);
            }

            //var booking = user.Booking;
            // todo Удалить CreatedTroubles
            var result = await userManager.DeleteAsync(user);
            return NoContent();
        }
    }
}
