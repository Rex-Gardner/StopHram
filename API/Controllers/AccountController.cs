using ClientModels.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Контроллер аутентификации
    /// </summary>
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager">Менеджер пользователей</param>
        /// <param name="signInManager">Менеджер аутентифицированных пользователей</param>
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="clientUserLogin">Пользовательские данные для входа</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/login")]
        public async Task<IActionResult> Login([FromBody]UserLogin clientUserLogin, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (clientUserLogin == null)
            {
                //var error = ServiceErrorResponses.BodyIsMissing(nameof(clientUserLogin));
                return BadRequest();
            }

            var modelUserLogin = ModelConverters.UserIdentity.UserLoginConverter.Convert(clientUserLogin);

            var result = await signInManager.PasswordSignInAsync(modelUserLogin.UserName, modelUserLogin.Password, modelUserLogin.RememberMe, false);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// Выход из текущей сессии
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/v1/logoff")]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            var usere = HttpContext.User;
            return Ok();
        }
    }
}
