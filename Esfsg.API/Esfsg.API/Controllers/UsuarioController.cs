using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class UsuarioController : ControllerBase
    {
        #region Construtor
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> ConsultarUsuarioLogin([FromQuery] string CPF)
        {
            try
            {
                var result = await _usuarioService.ConsultarUsuarioLogin(CPF);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
