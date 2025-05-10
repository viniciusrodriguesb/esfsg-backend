using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class ClasseController : ControllerBase
    {

        #region Construtor
        private readonly IClasseService _classeService;
        private readonly IMemoryCacheService _memoryCacheService;
        public ClasseController(IClasseService classeService,
                                IMemoryCacheService memoryCacheService)
        {
            _classeService = classeService;
            _memoryCacheService = memoryCacheService;
        } 
        #endregion

        [HttpGet("obter-todas")]
        public async Task<IActionResult> ConsultarClasses()
        {
            const string key = "classes-key";

            try
            {
                var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

                if (response is null)
                {
                    response = await _classeService.ConsultarClasses();

                    if (response == null || !response.Any())
                        return NotFound("Nenhum registro encontrado.");

                    _memoryCacheService.Set(key, response, TimeSpan.FromMinutes(30));
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
