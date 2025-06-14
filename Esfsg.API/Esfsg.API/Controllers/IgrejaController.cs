using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class IgrejaController : ControllerBase
    {
        #region Construtor
        private readonly IIgrejaService _igrejaService;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IWebHostEnvironment _env;
        public IgrejaController(IIgrejaService igrejaService,
                                IMemoryCacheService memoryCacheService,
                                IWebHostEnvironment env)
        {
            _igrejaService = igrejaService;
            _env = env;
            _memoryCacheService = memoryCacheService;
        }
        #endregion

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta as igrejas gravadas no banco de dados.")]
        public async Task<IActionResult> ConsultarIgrejas()
        {
            const string key = "igrejas-key";

            try
            {
                var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

                if (response is null || _env.IsDevelopment())
                {
                    response = await _igrejaService.ConsultarIgrejas();

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
        [SwaggerOperation(Summary = "Inclui uma nova igreja no banco de dados.")]
        public async Task<IActionResult> IncluirIgreja([FromBody] IgrejaRequest request)
        {
            try
            {
                await _igrejaService.IncluirIgreja(request);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "Exclui uma igreja no banco de dados.")]
        public async Task<IActionResult> ExcluirIgreja([FromQuery] int Id)
        {
            try
            {
                await _igrejaService.ExcluirIgreja(Id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edita uma igreja no banco de dados.")]
        public async Task<IActionResult> EditarIgreja([FromQuery] int Id, [FromBody] string NovoNome)
        {
            try
            {
                await _igrejaService.EditarIgreja(Id, NovoNome);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
