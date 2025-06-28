using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class InstrumentoController : ControllerBase
    {
        #region Construtor
        private readonly IWebHostEnvironment _env;
        private readonly IInstrumentoService _instrumentoService;
        private readonly IMemoryCacheService _memoryCacheService;
        public InstrumentoController(IInstrumentoService instrumentoService,
                                     IMemoryCacheService memoryCacheService,
                                     IWebHostEnvironment env)
        {
            _instrumentoService = instrumentoService;
            _memoryCacheService = memoryCacheService;
            _env = env;
        }
        #endregion

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta de todos os instrumentos no banco de dados.")]
        public async Task<IActionResult> ConsultarInstrumentos()
        {
            const string key = "instrumentos-key";

            try
            {
                var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

                if (response is null || _env.IsDevelopment())
                {
                    response = await _instrumentoService.ConsultarInstrumentos();

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
        [SwaggerOperation(Summary = "Inclui um novo instrumento no banco de dados.")]
        public async Task<IActionResult> IncluirInstrumento([FromBody] string Nome)
        {
            try
            {
                await _instrumentoService.IncluirInstrumento(Nome);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Exclui um instrumento no banco de dados.")]
        public async Task<IActionResult> ExcluirInstrumento([FromQuery] int Id)
        {
            try
            {
                await _instrumentoService.ExcluirInstrumento(Id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edita um instrumento no banco de dados.")]
        public async Task<IActionResult> EditarInstrumento([FromQuery] int Id, [FromBody] string NovoNome)
        {
            try
            {
                await _instrumentoService.EditarInstrumento(Id, NovoNome);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
