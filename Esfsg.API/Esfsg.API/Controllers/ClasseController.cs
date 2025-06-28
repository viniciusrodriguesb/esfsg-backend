using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class ClasseController : ControllerBase
    {

        #region Construtor
        private readonly IWebHostEnvironment _env;
        private readonly IClasseService _classeService;
        private readonly IMemoryCacheService _memoryCacheService;
        public ClasseController(IClasseService classeService,
                                IMemoryCacheService memoryCacheService,
                                IWebHostEnvironment env)
        {
            _env = env;
            _classeService = classeService;
            _memoryCacheService = memoryCacheService;
        }
        #endregion

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta as classes gravadas no banco de dados.")]
        public async Task<IActionResult> ConsultarClasses()
        {
            const string key = "classes-key";

            try
            {
                var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

                if (response is null || _env.IsDevelopment())
                {
                    response = await _classeService.ConsultarClasses();

                    if (response == null || !response.Any())
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

        [HttpPost]
        [SwaggerOperation(Summary = "Inclui uma nova classe no banco de dados.")]
        public async Task<IActionResult> IncluirClasse([FromBody] string Nome)
        {
            try
            {
                await _classeService.IncluirClasse(Nome);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Exclui uma classe no banco de dados.")]
        public async Task<IActionResult> ExcluirClasse([FromQuery] int Id)
        {
            try
            {
                await _classeService.ExcluirClasse(Id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edita uma classe no banco de dados.")]
        public async Task<IActionResult> EditarClasse([FromQuery] int Id, [FromBody] string NovoNome)
        {
            try
            {
                await _classeService.EditarClasse(Id, NovoNome);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
