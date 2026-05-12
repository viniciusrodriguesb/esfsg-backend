using Esfsg.Application;
using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class InscricaoController : ControllerBase
    {

        #region Construtor
        private readonly IInscricaoService _inscricaoService;
        private readonly IStatusService _statusService;
        private readonly IMemoryCacheService _memoryCacheService;
        public InscricaoController(IInscricaoService inscricaoService,
                                IMemoryCacheService memoryCacheService,
                                IStatusService statusService)
        {
            _inscricaoService = inscricaoService;
            _memoryCacheService = memoryCacheService;
            _statusService = statusService;
        }
        #endregion        

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta da inscrição do usuário no evento.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConsultarInscricao([FromQuery] InscricaoEventoResquest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados de entrada inválidos", erros = ModelState.Values.SelectMany(v => v.Errors) });

            if (request.IdUsuario <= 0 || request.IdEvento <= 0)
                return BadRequest(new { mensagem = "IdUsuario e IdEvento devem ser valores positivos" });

            try
            {
                var result = await _inscricaoService.ConsultarInscricao(request);
                if (result == null)
                    return NotFound(new { mensagem = "Nenhuma inscrição encontrada para os parâmetros informados" });

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { mensagem = "Erro ao consultar inscrição", erro = ex.Message });
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Realização da inscrição no evento solicitado.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RealizarInscricao([FromBody] InscricaoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados de entrada inválidos", erros = ModelState.Values.SelectMany(v => v.Errors) });

            try
            {
                var result = await _inscricaoService.RealizarInscricao(request);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { mensagem = "Erro ao realizar inscrição", erro = ex.Message });
            }
        }

        [HttpPut("cancelar/{id}")]
        [SwaggerOperation(Summary = "Cancelamento da inscrição no evento solicitado.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelarInscricao([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { mensagem = "ID da inscrição deve ser um valor positivo" });

            try
            {
                await _statusService.AtualizarStatusInscricao(StatusEnum.CANCELADA, id);
                return Ok(new { mensagem = "Inscrição cancelada com sucesso" });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { mensagem = "Erro ao cancelar inscrição", erro = ex.Message });
            }
        }

    }
}
