using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class CheckInController : ControllerBase
    {

        #region Construtor
        private readonly ICheckInService _checkInService;
        public CheckInController(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }
        #endregion

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta de inscrições para marcar presença.")]
        public async Task<IActionResult> Consultar([FromQuery] ConsultaCheckInRequest request, [FromQuery] PaginacaoRequest paginacao)
        {
            try
            {
                var response = await _checkInService.Consultar(request, paginacao);

                if (response.Itens == null || !response.Itens.Any())
                    return NotFound("Nenhum registro encontrado.");

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Confirmação de presença no evento.")]
        public async Task<IActionResult> ConfirmarPresenca([FromBody] ValidaPresencaRequest request)
        {
            try
            {
                var result = await _checkInService.ConfirmarPresenca(request);
                return result.Sucesso ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
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
