using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Consultar([FromQuery] ConsultaCheckInRequest request)
        {
            try
            {
                var response = await _checkInService.Consultar(request);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarPresenca([FromBody] ValidaPresencaRequest request)
        {
            try
            {
                await _checkInService.ConfirmarPresenca(request);
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
