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
        /// 
        /// </summary>
        /// <param name="roleManager">Менеджер ролей</param>
        /// <param name="userManager">Менеджер пользователей</param>
        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="clientPatchInfo">Модель изменения пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{userName}")]
        public async Task<IActionResult> PatchRoleAsync([FromRoute]string userName, [FromBody] Client.RoleUserPatchInfo clientPatchInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (clientPatchInfo == null)
            {
                //var error = ServiceErrorResponses.BodyIsMissing(nameof(clientPatchInfo));
                return BadRequest();
            }

            RoleUserPatchInfo modelPatchInfo;
            try
            {
                modelPatchInfo = ModelConverters.Roles.RoleUserPatchInfoConverter.Converter(userName, clientPatchInfo);
            }
            catch (ArgumentNullException ex)
            {
                //var error = ServiceErrorResponses.ValidationError(ex.Message);
                return BadRequest();
            }

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.UserNotFound(clientPatchInfo.UserId);
                //return BadRequest(error);
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
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.RoleNotFound(clientPatchInfo.UserRoles.ToString());
                //return BadRequest(error);
            }

            user.Roles = modelUserRoles;

            await userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roles = roleManager.Roles;
            IReadOnlyList<Client.Role> clientRoles;
            try
            {
                clientRoles = roles.Select(item => ModelConverters.Roles.RoleConverter.Convert(item)).ToImmutableList();
            }
            catch (ArgumentNullException)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.RoleNotFound("roles");
                //return NotFound(error);
            }

            return Ok(clientRoles);
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetRole([FromRoute] string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var role = await roleManager.FindByNameAsync(name);
            try
            {
                var clientRoles = ModelConverters.Roles.RoleConverter.Convert(role);
            }
            catch (ArgumentNullException)
            {
                throw new NotImplementedException();
                //var error = ServiceErrorResponses.RoleNotFound("role");
                //return NotFound(error);
            }

            return Ok(role);
        }
    }
}
