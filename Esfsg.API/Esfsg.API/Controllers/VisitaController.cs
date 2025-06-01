using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> ConsultarInscritosVisita(int IdEvento)
        {
            try
            {
                var result = await _visitaService.ConsultarInscritosVisita(IdEvento);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("funcoes")]
        public IActionResult ConsultarFuncoesVisita()
        {
            try
            {
                var result = _visitaService.ConsultarFuncoesVisita();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("alocar")]
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
