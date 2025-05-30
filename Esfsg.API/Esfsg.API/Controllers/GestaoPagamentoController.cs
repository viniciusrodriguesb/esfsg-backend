using Esfsg.Application.Enum;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> ObterDadosPagamentoInscricao([FromQuery] string? Nome, int IdEvento)
        {
            try
            {
                var result = await _gestaoPagamentoService.ObterDadosPagamentoInscricao(Nome, IdEvento);
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
        public async Task<IActionResult> BuscarInscricaoPagamentoPorId([FromBody] int IdInscricao)
        {
            try
            {
                await _gestaoPagamentoService.GerarNovoCodigoPix(IdInscricao);
                return StatusCode(StatusCodes.Status201Created);
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

        [HttpPut("confirmacao-manual")]
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
