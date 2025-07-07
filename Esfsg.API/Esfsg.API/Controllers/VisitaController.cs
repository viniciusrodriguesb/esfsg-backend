using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
            try
            {
                var result = await _visitaService.ConsultarInscritosVisita(request, Paginacao);

                if (result.Itens == null || !result.Itens.Any())
                    return NotFound("Nenhum registro encontrado.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("funcoes")]
        [SwaggerOperation(Summary = "Consulta todas as funções para realizar na visita.")]
        public IActionResult ConsultarFuncoesVisita()
        {
            try
            {
                var result = _visitaService.ConsultarFuncoesVisita();

                if (result == null || !result.Any())
                    return NotFound("Nenhum registro encontrado.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta todas as visitas criadas.")]
        public async Task<IActionResult> ConsultarVisitas()
        {
            const string key = "visitas-key";

            try
            {
                var response = _memoryCacheService.Get<List<VisitaResponse>>(key);

                if (response is null)
                {
                    response = await _visitaService.ConsultarVisitas();

                    if (response == null || !response.Any())
                        return NotFound("Nenhum registro encontrado.");

                    _memoryCacheService.Set(key, response, TimeSpan.FromMinutes(60));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("alocar")]
        [SwaggerOperation(Summary = "Alocar um inscrito na visita desejada.")]
        public async Task<IActionResult> AlocarInscritosVisita([FromBody] List<AlocarVisitaRequest> alocacoes)
        {
            try
            {
                var result = await _visitaService.AlocarInscritosVisita(alocacoes);
                return result.Sucesso ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("criar")]
        [SwaggerOperation(Summary = "Inclusão de uma nova visita")]
        public async Task<IActionResult> CriarVisita([FromBody] VisitaRequest visita)
        {
            try
            {
                var result = await _visitaService.CriarVisita(visita);
                return result.Sucesso ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Exclusão de uma visita no banco de dados.")]
        public async Task<IActionResult> ExcluirVisita(int Id)
        {
            try
            {
                await _visitaService.ExcluirVisita(Id);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
