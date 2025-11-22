using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class VisitaController : ControllerBase
    {
        #region Construtor
        private readonly IVisitaService _visitaService;
        private readonly IMemoryCacheService _memoryCacheService;
        public VisitaController(IVisitaService visitaService, IMemoryCacheService memoryCacheService)
        {
            _visitaService = visitaService;
            _memoryCacheService = memoryCacheService;
        }
        #endregion

        [HttpGet("inscritos-visitas")]
        [SwaggerOperation(Summary = "Consulta todos os inscritos ativos para visita.")]
        public async Task<IActionResult> ConsultarInscritosVisita([FromQuery] ConsultaVisitaRequest request, [FromQuery] PaginacaoRequest Paginacao)
        {
            var result = await _visitaService.ConsultarInscritosVisita(request, Paginacao);

            if (result.Itens == null || !result.Itens.Any())
                return NotFound("Nenhum registro encontrado.");

            return Ok(result);

        }

        [HttpGet("funcoes")]
        [SwaggerOperation(Summary = "Consulta todas as funções para realizar na visita.")]
        public IActionResult ConsultarFuncoesVisita()
        {
            var result = _visitaService.ConsultarFuncoesVisita();

            if (result == null || !result.Any())
                return NotFound("Nenhum registro encontrado.");

            return Ok(result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta todas as visitas criadas.")]
        public async Task<IActionResult> ConsultarVisitas([FromQuery] string? Descricao)
        {
            var response = await _visitaService.ConsultarVisitas(Descricao);

            if (response == null || !response.Any())
                return NotFound("Nenhum registro encontrado.");

            return Ok(response);
        }

        [HttpPost("alocar")]
        [SwaggerOperation(Summary = "Alocar um inscrito na visita desejada.")]
        public async Task<IActionResult> AlocarInscritosVisita([FromBody] List<AlocarVisitaRequest> alocacoes)
        {
            var result = await _visitaService.AlocarInscritosVisita(alocacoes);
            return result.Sucesso ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }

        [HttpPost("criar")]
        [SwaggerOperation(Summary = "Inclusão de uma nova visita")]
        public async Task<IActionResult> CriarVisita([FromBody] VisitaRequest visita)
        {
            var result = await _visitaService.CriarVisita(visita);
            return result.Sucesso ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edição de uma visita")]
        public async Task<IActionResult> EditarVisita([FromBody] EditarVisitaRequest visita)
        {
            var result = await _visitaService.EditarVisitaAsync(visita);
            return result.Sucesso ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Exclusão de uma visita no banco de dados.")]
        public async Task<IActionResult> ExcluirVisita(int Id)
        {
            await _visitaService.ExcluirVisita(Id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
