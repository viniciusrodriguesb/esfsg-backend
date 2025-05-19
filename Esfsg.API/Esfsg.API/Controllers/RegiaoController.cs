using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class RegiaoController : ControllerBase
    {

        #region Construtor
        private readonly IWebHostEnvironment _env;
        private readonly IRegiaoService _regiaoService;
        private readonly IMemoryCacheService _memoryCacheService;
        public RegiaoController(IRegiaoService regiaoService,
                                IMemoryCacheService memoryCacheService,
                                IWebHostEnvironment env)
        {
            _regiaoService = regiaoService;
            _memoryCacheService = memoryCacheService;
            _env = env;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> ConsultarRegioes()
        {
            const string key = "regioes-key";

            try
            {
                var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

                if (response is null || _env.IsDevelopment())
                {
                    response = await _regiaoService.ConsultarRegioes();

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
