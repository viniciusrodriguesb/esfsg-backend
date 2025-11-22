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
            var response = await _checkInService.Consultar(request, paginacao);

            if (response.Itens == null || !response.Itens.Any())
                return NotFound("Nenhum registro encontrado.");

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Confirmação de presença no evento por lista de ID's.")]
        public async Task<IActionResult> ConfirmarPresenca([FromBody] ValidaPresencaIdRequest request)
        {
            var result = await _checkInService.ConfirmarPresencaPorId(request);
            return result.Sucesso ? StatusCode(StatusCodes.Status200OK, result) : StatusCode(StatusCodes.Status400BadRequest, result);
        }
    }
}
