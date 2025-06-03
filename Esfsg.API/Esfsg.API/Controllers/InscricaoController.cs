using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Enums;
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
        private readonly IStatusService _statusService;
        private readonly IMemoryCacheService _memoryCacheService;
        public InscricaoController(IInscricaoService inscricaoService,
                                IMemoryCacheService memoryCacheService,
                                IStatusService statusService)
        {
            _inscricaoService = inscricaoService;
            _memoryCacheService = memoryCacheService;
            _statusService = statusService;
        }
        #endregion        

        [HttpGet]
        public async Task<IActionResult> ConsultarInscricao([FromQuery] InscricaoEventoResquest request)
        {
            try
            {
                var result = await _inscricaoService.ConsultarInscricao(request);

                if (result == null)
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

        [HttpPut("cancelar")]
        public async Task<IActionResult> CancelarInscricao(int Id)
        {
            try
            {
                await _statusService.AtualizarStatusInscricao(StatusEnum.CANCELADA, Id);
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
