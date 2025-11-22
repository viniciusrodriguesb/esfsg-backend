using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Consulta dados do usuário para realizar LOGIN.")]
        public async Task<IActionResult> ConsultarUsuarioLogin([FromQuery] string CPF)
        {
            var result = await _usuarioService.ConsultarUsuarioLogin(CPF);

            if (result == null)
                return NotFound("Nenhum registro encontrado.");

            return Ok(result);
        }

        [HttpGet("administrativo")]
        [SwaggerOperation(Summary = "Consulta dados do usuário para realizar LOGIN ADMINISTRATIVO.")]
        public async Task<IActionResult> ConsultarUsuarioAdmnistrativo([FromQuery] UsuarioAdministrativoRequest request)
        {
            var result = await _usuarioService.ConsultarUsuarioAdministrativo(request);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Alteração de dados do usuário")]
        public async Task<IActionResult> AlterarUsuario([FromBody] AlterarUsuarioRequest request)
        {
            var result = await _usuarioService.AlterarUsuario(request);
            return result.Sucesso ? Ok(result) : BadRequest(result);
        }

    }
}
