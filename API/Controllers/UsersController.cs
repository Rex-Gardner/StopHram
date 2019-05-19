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
using API.Helpers;
using Client = ClientModels.Users;
using Model = Models.Users;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер пользователей
    /// </summary>
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public UsersController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="clientCreationInfo">Модель создания пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] Client.UserCreationInfo clientCreationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (clientCreationInfo == null)
            {
                var error = Responses.BodyIsMissing(nameof(clientCreationInfo));
                return BadRequest(error);
            }

            var modelCreationInfo = ModelConverters.Users.UserCreationInfoConverter.Convert(clientCreationInfo);

            var user = await userManager.FindByNameAsync(modelCreationInfo.UserName);
            if (user != null)
            {
                var error = Responses.DuplicationError($"User with username {modelCreationInfo.UserName} already use", "User");
                return BadRequest();
            }

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
                var error = Responses.InvalidData(result.Errors.First().ToString(), "NotValid");
                return BadRequest(error);
            }

            await userManager.AddToRoleAsync(modelUser, "user");
            var clientUser = ModelConverters.Users.UserConverter.Convert(modelUser);
            return CreatedAtRoute("GetUserRoute", new { userName = clientUser.UserName }, clientUser);
        }

        /// <summary>
        /// Получение списка полльзователей
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "admin")]
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
                var error = Responses.NotFoundError("Users not found", "userManager");
                return NotFound(error);
            }

            return Ok(clientUsers);
        }

        /// <summary>
        /// Получение пользователя по логину
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userName}", Name = "GetUserRoute")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromRoute]string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var isAuthorize = HttpContext.User.IsInRole("admin") ||
                              HttpContext.User.Identity.Name.CompareTo(userName.ToLower()) == 0;

            if (!isAuthorize)
            {
                return Forbid();
            }

            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                var error = Responses.NotFoundError($"User with {userName} not found", "User");
                return BadRequest(error);
            }

            var clientUser = ModelConverters.Users.UserConverter.Convert(user);
            return Ok(clientUser);
        }

        /// <summary>
        /// Модификация пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="clientPatchInfo">Модель изменения пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{userName}")]
        [Authorize]
        public async Task<IActionResult> PatchUserAsync([FromRoute] string userName, [FromBody] Client.UserPatchInfo clientPatchInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (clientPatchInfo == null)
            {
                var error = Responses.BodyIsMissing(nameof(clientPatchInfo));
                return BadRequest(error);
            }

            var isAuthorize = HttpContext.User.IsInRole("admin") ||
                              HttpContext.User.Identity.Name.CompareTo(userName.ToLower()) == 0;

            if (!isAuthorize)
            {
                return Forbid();
            }

            var modelPatchInfo = ModelConverters.Users.UserPatchInfoConverter.Convert(userName, clientPatchInfo);

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest();
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

            if (updated)
            {
                await userManager.UpdateAsync(user);
            }

            return Ok();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("{userName}")]
        public async Task<ActionResult> Delete([FromRoute]string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var isAuthorize = HttpContext.User.IsInRole("admin") ||
                              HttpContext.User.Identity.Name.CompareTo(userName.ToLower()) == 0;

            if (!isAuthorize)
            {
                return Forbid();
            }

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                var error = Responses.NotFoundError($"User with username {userName} not found", "User");
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);
            return NoContent();
        }
    }
}
