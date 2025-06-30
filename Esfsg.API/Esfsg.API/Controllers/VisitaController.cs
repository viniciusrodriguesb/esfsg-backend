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
        public VisitaController(IVisitaService visitaService)
        {
            _visitaService = visitaService;
        }
        #endregion

        [HttpGet("inscritos-visitas")]
        [SwaggerOperation(Summary = "Consulta todos os inscritos ativos para visita.")]
        public async Task<IActionResult> ConsultarInscritosVisita(int IdEvento, [FromQuery] PaginacaoRequest Paginacao)
        {
            try
            {
                var result = await _visitaService.ConsultarInscritosVisita(IdEvento, Paginacao);

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
            catch(Exception ex)
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
                await _visitaService.AlocarInscritosVisita(alocacoes);
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

        [HttpPost("criar")]
        [SwaggerOperation(Summary = "Inclusão de uma nova visita")]
        public async Task<IActionResult> CriarVisita([FromBody] VisitaRequest visita)
        {
            try
            {
                await _visitaService.CriarVisita(visita);
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
