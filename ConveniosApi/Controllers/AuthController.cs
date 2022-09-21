using ConveniosApi.Helpers;
using ConveniosApi.Models;
using ConveniosApi.Services;
using ConveniosApi.ServiciosAcromaxEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ConveniosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IStringLocalizer<AuthController> _localizer;
        private readonly IUserService _userService;
        private readonly servicios_acromaxContext _context;

        public AuthController(servicios_acromaxContext context, IUserService userService, IStringLocalizer<AuthController> localizer)
        {
            _localizer = localizer;
            _userService = userService;
            _context = context;
        }

        /// <summary>
        /// Login del usuario
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(LoginData login)
        {
            try
            {
                var response = await _userService.GenerateToken(_context, login);

                if (response == null)
                    throw new Exception("login_invalido");

                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(FunctionsHelper.BadRequest(ex,_localizer));
            }
        }
    }
}
