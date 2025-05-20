using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class InscricaoController : ControllerBase
    {

        #region Construtor
        private readonly IInscricaoService _inscricaoService;
        private readonly IMemoryCacheService _memoryCacheService;
        public InscricaoController(IInscricaoService inscricaoService,
                                IMemoryCacheService memoryCacheService)
        {
            _inscricaoService = inscricaoService;
            _memoryCacheService = memoryCacheService;
        }
        #endregion        

        [HttpPost]
        public async Task<IActionResult> RealizarInscricao([FromBody] InscricaoRequest request)
        {
            try
            {
                await _inscricaoService.RealizarInscricao(request);
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

        [HttpPut]
        public async Task<IActionResult> CancelarInscricao(int Id)
        {
            try
            {
                await _inscricaoService.CancelarInscricao(Id);
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

        [HttpGet]
        public async Task<IActionResult> ConsultarInscricao([FromQuery] InscricaoEventoResquest request)
        {
            try
            {
                var result = await _inscricaoService.ConsultarInscricao(request);
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

    }
}
