using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Consulta a todas as regiões.")]
        public async Task<IActionResult> ConsultarRegioes()
        {
            const string key = "regioes-key";

            var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);
            if (response is null || _env.IsDevelopment())
            {
                response = await _regiaoService.ConsultarRegioes();

                if (response == null || !response.Any())
                    return NotFound("Nenhum registro encontrado.");

                _memoryCacheService.Set(key, response, TimeSpan.FromMinutes(60));
            }

            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Inclui uma nova região no banco de dados.")]
        public async Task<IActionResult> IncluirRegiao([FromBody] string Nome)
        {
            await _regiaoService.IncluirRegiao(Nome);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Exclui uma região no banco de dados.")]
        public async Task<IActionResult> ExcluirRegiao([FromQuery] int Id)
        {
            await _regiaoService.ExcluirRegiao(Id);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edita uma região no banco de dados.")]
        public async Task<IActionResult> EditarRegiao([FromQuery] int Id, [FromBody] string NovoNome)
        {
            await _regiaoService.EditarRegiao(Id, NovoNome);
            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}
