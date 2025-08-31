using ApplicationLayer.Services.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("getToken")]
        public async Task<ActionResult<string>> GetToken(string correo, int edad)
            => await _authService.GetToken(correo, edad);
    }
}
