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
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarPresenta([FromBody] ValidaPresencaRequest request)
        {
            try
            {
                await _checkInService.ConfirmarPresenca(request);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
