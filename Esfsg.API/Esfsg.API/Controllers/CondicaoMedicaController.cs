using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class CondicaoMedicaController : ControllerBase
    {

        #region Construtor
        private readonly IWebHostEnvironment _env;
        private readonly ICondicaoMedicaService _condicaoMedicaService;
        private readonly IMemoryCacheService _memoryCacheService;
        public CondicaoMedicaController(ICondicaoMedicaService condicaoMedicaService,
                                        IMemoryCacheService memoryCacheService,
                                        IWebHostEnvironment env)
        {
            _condicaoMedicaService = condicaoMedicaService;
            _memoryCacheService = memoryCacheService;
            _env = env;
        }
        #endregion

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta condições médicas no banco de dados.")]
        public async Task<IActionResult> ConsultarCondicoesMedicas()
        {
            const string key = "condicoes-medicas-key";

            var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

            if (response is null || _env.IsDevelopment())
            {
                response = await _condicaoMedicaService.ConsultarCondicoesMedicas();

                if (response == null || !response.Any())
                    return NotFound("Nenhum registro encontrado.");

                _memoryCacheService.Set(key, response, TimeSpan.FromMinutes(60));
            }

            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Inclui uma nova condição médica no banco de dados.")]
        public async Task<IActionResult> IncluirCondicaoMedica([FromBody] string Nome)
        {
            await _condicaoMedicaService.IncluirCondicaoMedica(Nome);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Exclui uma condição médica no banco de dados.")]
        public async Task<IActionResult> ExcluirCondicaoMedica([FromQuery] int Id)
        {
            await _condicaoMedicaService.ExcluirCondicaoMedica(Id);
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edita uma condição médica no banco de dados.")]
        public async Task<IActionResult> EditarCondicaoMedica([FromQuery] int Id, [FromBody] string NovoNome)
        {
            await _condicaoMedicaService.EditarCondicaoMedica(Id, NovoNome);
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}
