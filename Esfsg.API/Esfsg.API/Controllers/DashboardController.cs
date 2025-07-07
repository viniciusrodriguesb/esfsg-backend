using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class DashboardController : ControllerBase
    {

        #region Construtor
        private readonly IDashboardService _dashboardService;
        private readonly IMemoryCacheService _memoryCacheService;
        public DashboardController(IDashboardService dashboardService, IMemoryCacheService memoryCache)
        {
            _dashboardService = dashboardService;
            _memoryCacheService = memoryCache;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> ConsultarDadosDashboard([FromQuery] int IdEvento)
        {
            string key = $"dashboard-{IdEvento}-key";

            try
            {
                var response = _memoryCacheService.Get<DashboardResponse>(key);

                if (response is null)
                {
                    response = await _dashboardService.ConsultarDadosDashboard(IdEvento);

                    if (response == null)
                        return NotFound("Nenhum registro encontrado.");

                    _memoryCacheService.Set(key, response, TimeSpan.FromMinutes(10));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("evento-proximo")]
        public async Task<IActionResult> ConsultarEventoProximo([FromQuery] int IdRegiao)
        {
            const string key = "evento-proximo-key";

            try
            {
                var response = _memoryCacheService.Get<EventoProximoResponse>(key);

                if (response is null)
                {
                    response = await _dashboardService.ConsultarEventoProximo(IdRegiao);

                    if (response == null)
                        return NotFound("Nenhum registro encontrado.");

                    _memoryCacheService.Set(key, response, TimeSpan.FromMinutes(60));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
