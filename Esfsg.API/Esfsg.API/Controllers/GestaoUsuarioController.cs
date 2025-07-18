using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class GestaoUsuarioController : ControllerBase
    {
        #region Construtor
        private readonly IGestaoUsuarioService _gestaoUsuarioService;
        public GestaoUsuarioController(IGestaoUsuarioService gestaoUsuarioService)
        {
            _gestaoUsuarioService = gestaoUsuarioService;
        }
        #endregion

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta todos os usuários cadastrados no sistema.")]
        public async Task<IActionResult> ConsultarUsuarioLogin([FromQuery] GestaoUsuarioRequest request, [FromQuery] PaginacaoRequest paginacao)
        {
            try
            {
                var result = await _gestaoUsuarioService.ConsultarUsuarios(request, paginacao);

                if (result == null)
                    return NotFound("Nenhum registro encontrado.");

                return Ok(result);
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
                var result = await _gestaoUsuarioService.AlterarRoleUsuario(role);
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
                var result = await _gestaoUsuarioService.AlterarSenha(request);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
