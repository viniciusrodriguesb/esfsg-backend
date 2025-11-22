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
        public async Task<IActionResult> ConsultarInscricoesParaLiberacao([FromQuery] InscricoesPendentesRequest request, [FromQuery] PaginacaoRequest Paginacao)
        {
            var result = await _gestaoInscricaoService.ConsultarInscricoesParaLiberacao(request, Paginacao);
            return Ok(result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta com filtro de todas as pessoas inscritas.")]
        public async Task<IActionResult> ConsultarInscricoes([FromQuery] FiltroGestaoInscricaoRequest Filtro, [FromQuery] PaginacaoRequest Paginacao)
        {
            var result = await _gestaoInscricaoService.ConsultarInscricoes(Filtro, Paginacao);
            return Ok(result);
        }

        [HttpPost("aprovar")]
        [SwaggerOperation(Summary = "Liberação das inscrições para participação do evento.")]
        public async Task<IActionResult> AprovarInscricao([FromBody] List<int> Ids)
        {
            if (Ids == null || !Ids.Any())
                return StatusCode(StatusCodes.Status400BadRequest, "Nenhuma inscrição foi informada para aprovação.");

            foreach (var id in Ids)
            {
                await _statusService.AtualizarStatusInscricao(StatusEnum.AGUARDANDO_PAGAMENTO, id);
            }

            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("cancelar")]
        [SwaggerOperation(Summary = "Cancelamento das inscrições para participação do evento.")]
        public async Task<IActionResult> CancelarInscricao([FromBody] List<int> Ids)
        {
            if (Ids == null || !Ids.Any())
                return StatusCode(StatusCodes.Status400BadRequest, "Nenhuma inscrição foi informada para aprovação.");

            foreach (var id in Ids)
            {
                await _statusService.AtualizarStatusInscricao(StatusEnum.CANCELADA, id);
            }

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
