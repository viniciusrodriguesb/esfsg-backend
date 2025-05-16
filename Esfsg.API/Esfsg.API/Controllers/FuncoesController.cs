using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class FuncoesController : ControllerBase
    {

        #region Construtor
        private readonly IWebHostEnvironment _env;
        private readonly IFuncoesService _funcoesService;
        private readonly IMemoryCacheService _memoryCacheService;
        public FuncoesController(IFuncoesService funcoesService,
                                IMemoryCacheService memoryCacheService,
                                IWebHostEnvironment env)
        {
            _funcoesService = funcoesService;
            _memoryCacheService = memoryCacheService;
            _env = env;
        }
        #endregion

        [HttpGet("igreja")]
        public async Task<IActionResult> ConsultarFuncoesIgreja()
        {
            const string key = "funcoes-igreja-key";

            try
            {
                var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

                if (response is null || _env.IsDevelopment())
                {
                    response = await _funcoesService.ConsultarFuncoesIgreja();

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

        [HttpGet("evento")]
        public async Task<IActionResult> ConsultarFuncoesEvento()
        {
            const string key = "funcoes-evento-key";

            try
            {
                var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

                if (response is null)
                {
                    response = await _funcoesService.ConsultarFuncoesEvento();

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
