using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class GestaoInscricaoController : ControllerBase
    {

        #region Construtor
        private readonly IGestaoInscricaoService _gestaoInscricaoService;
        private readonly IStatusService _statusService;
        public GestaoInscricaoController(IGestaoInscricaoService gestaoInscricaoService, IStatusService statusService)
        {
            _gestaoInscricaoService = gestaoInscricaoService;
            _statusService = statusService;
        }
        #endregion

        [HttpGet("pendentes")]
        [SwaggerOperation(Summary = "Consulta inscrições pendentes de liberação, permitidas para o CPF logado visualizar.")]
        public async Task<IActionResult> ConsultarInscricoesParaLiberacao([FromQuery] string Cpf, [FromQuery] PaginacaoRequest Paginacao)
        {
            try
            {
                var result = await _gestaoInscricaoService.ConsultarInscricoesParaLiberacao(Cpf, Paginacao);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta com filtro de todas as pessoas inscritas.")]
        public async Task<IActionResult> ConsultarInscricoes([FromQuery] FiltroGestaoInscricaoRequest Filtro, [FromQuery] PaginacaoRequest Paginacao)
        {
            try
            {
                var result = await _gestaoInscricaoService.ConsultarInscricoes(Filtro, Paginacao);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("aprovar")]
        [SwaggerOperation(Summary = "Liberação da inscrição para participação do evento.")]
        public async Task<IActionResult> AprovarInscricao([FromBody] List<int> Ids)
        {
            try
            {
                foreach (var id in Ids)
                {
                    await _statusService.AtualizarStatusInscricao(StatusEnum.AGUARDANDO_PAGAMENTO, id);
                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
