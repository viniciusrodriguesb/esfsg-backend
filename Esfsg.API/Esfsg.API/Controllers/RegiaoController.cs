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

        [HttpPost]
        public async Task<IActionResult> IncluirRegiao([FromBody] string Nome)
        {
            try
            {
                await _regiaoService.ConsultarRegioes();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirRegiao([FromQuery] int Id)
        {
            try
            {
                await _regiaoService.ConsultarRegioes();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarRegiao([FromQuery] int Id, [FromBody] string NovoNome)
        {
            try
            {
                await _regiaoService.ConsultarRegioes();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
