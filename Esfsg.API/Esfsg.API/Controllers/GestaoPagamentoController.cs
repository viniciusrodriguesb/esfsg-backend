using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class GestaoPagamentoController : ControllerBase
    {
        #region Construtor
        private readonly IGestaoPagamentoService _gestaoPagamentoService;
        private readonly IStatusService _statusService;
        public GestaoPagamentoController(IGestaoPagamentoService gestaoPagamentoService, IStatusService statusService)
        {
            _gestaoPagamentoService = gestaoPagamentoService;
            _statusService = statusService;
        }
        #endregion        

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta inscrições pendentes de pagamento e pagas.")]
        public async Task<IActionResult> ObterDadosPagamentoInscricao([FromQuery] ConsultaGestaoPagamentoRequest request, [FromQuery] PaginacaoRequest paginacao)
        {
            try
            {
                var result = await _gestaoPagamentoService.ObterDadosPagamentoInscricao(request, paginacao);

                if (result == null || !result.Itens.Any())
                    return NotFound("Nenhum registro encontrado.");

                return StatusCode(StatusCodes.Status200OK, result);
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

        [HttpPost("Pix/Gerar")]
        [SwaggerOperation(Summary = "Geração de novo código pix.")]
        public async Task<IActionResult> GerarNovoCodigoPix([FromBody] int IdInscricao)
        {
            try
            {
                var result = await _gestaoPagamentoService.GerarNovoCodigoPix(IdInscricao);
                return result.Sucesso ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("confirmacao-manual")]
        [SwaggerOperation(Summary = "Confirmação manual do pagamento da inscrição.")]
        public async Task<IActionResult> ConfirmarPagamento(int IdInscricao)
        {
            try
            {
                await _statusService.AtualizarStatusInscricao(StatusEnum.PAGAMENTO_CONFIRMADO, IdInscricao);
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
