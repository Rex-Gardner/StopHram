using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Roles;
using Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Model = Models.Roles;
using Client = ClientModels.Roles;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Immutable;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер ролей
    /// </summary>
    [Route("api/v1/roles")]
    public class RolesController : ControllerBase
    {
        private RoleManager<Role> roleManager;
        private UserManager<User> userManager;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="roleManager">Менеджер ролей</param>
        /// <param name="userManager">Менеджер пользователей</param>
        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// Изменение роли пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="clientPatchInfo">Модель изменения пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{userName}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PatchRoleAsync([FromRoute]string userName, [FromBody] Client.RoleUserPatchInfo clientPatchInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (clientPatchInfo == null)
            {
                var error = Responses.BodyIsMissing(nameof(clientPatchInfo));
                return BadRequest();
            }

            RoleUserPatchInfo modelPatchInfo;
            try
            {
                modelPatchInfo = ModelConverters.Roles.RoleUserPatchInfoConverter.Converter(userName, clientPatchInfo);
            }
            catch (ArgumentNullException ex)
            {
                var error = Responses.BodyIsMissing(ex.Message);
                return BadRequest(error);
            }

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                var error = Responses.NotFoundError("User not found", userName);
                return BadRequest();
            }

            List<string> modelUserRoles = new List<string>();

            foreach (var role in clientPatchInfo.UserRoles)
            {
                var tempRole = await roleManager.FindByNameAsync(role);
                if (tempRole != null)
                {
                    modelUserRoles.Add(role.ToLower());
                }
            }

            if (modelUserRoles.Count == 0)
            {
                var error = Responses.NotFoundError(nameof(clientPatchInfo.UserRoles), "UserRoles");
                return BadRequest(error);
            }

            user.Roles = modelUserRoles;

            await userManager.UpdateAsync(user);

            return Ok();
        }

        /// <summary>
        /// Получение списка ролей
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roles = roleManager.Roles;
            IReadOnlyList<Client.Role> clientRoles;
            try
            {
                clientRoles = roles.Select(item => ModelConverters.Roles.RoleConverter.Convert(item)).ToImmutableList();
            }
            catch (ArgumentNullException ex)
            {
                var error = Responses.NotFoundError(ex.Message, "roles");
                return NotFound();
            }

            return Ok(clientRoles);
        }

        /// <summary>
        /// Получение модели роли по названию
        /// </summary>
        /// <param name="name">Название роли</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetRole([FromRoute] string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var role = await roleManager.FindByNameAsync(name);
            try
            {
                var clientRoles = ModelConverters.Roles.RoleConverter.Convert(role);
            }
            catch (ArgumentNullException ex)
            {
                var error = Responses.NotFoundError(ex.Message, "role");
                return NotFound();
            }

            return Ok(role);
        }
    }
}
