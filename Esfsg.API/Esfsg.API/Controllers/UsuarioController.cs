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
            try
            {
                var result = await _usuarioService.ConsultarUsuarioLogin(CPF);

                if (result == null)
                    return NotFound("Nenhum registro encontrado.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("administrativo")]
        [SwaggerOperation(Summary = "Consulta dados do usuário para realizar LOGIN ADMINISTRATIVO.")]
        public async Task<IActionResult> ConsultarUsuarioAdmnistrativo([FromQuery] UsuarioAdministrativoRequest request)
        {
            try
            {
                var result = await _usuarioService.ConsultarUsuarioAdministrativo(request);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("administrativo/role")]
        [SwaggerOperation(Summary = "Alteração de role de um usuário.")]
        public async Task<IActionResult> AlterarRoleUsuario([FromBody] AlteraRoleRequest role)
        {
            try
            {
                var result = await _usuarioService.AlterarRoleUsuario(role);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("administrativo/senha")]
        [SwaggerOperation(Summary = "Redefinição de senha de um usuário.")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaRequest request)
        {
            try
            {
                var result = await _usuarioService.AlterarSenha(request);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
