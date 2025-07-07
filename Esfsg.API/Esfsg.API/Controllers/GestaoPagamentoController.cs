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
        public async Task<IActionResult> ObterDadosPagamentoInscricao([FromQuery] string? Nome, int IdEvento)
        {
            try
            {
                var result = await _gestaoPagamentoService.ObterDadosPagamentoInscricao(Nome, IdEvento);

                if (result == null || !result.Any())
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
        public async Task<IActionResult> BuscarInscricaoPagamentoPorId([FromBody] int IdInscricao)
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
        public async Task<IActionResult> ConfirmarPagamento(int Id)
        {
            try
            {
                await _statusService.AtualizarStatusInscricao(StatusEnum.PAGAMENTO_CONFIRMADO, Id);
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
