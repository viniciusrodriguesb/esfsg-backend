using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class CondicaoMedicaController : ControllerBase
    {

        #region Construtor
        private readonly ICondicaoMedicaService _condicaoMedicaService;
        private readonly IMemoryCacheService _memoryCacheService;
        public CondicaoMedicaController(ICondicaoMedicaService condicaoMedicaService,
                                     IMemoryCacheService memoryCacheService)
        {
            _condicaoMedicaService = condicaoMedicaService;
            _memoryCacheService = memoryCacheService;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> ConsultarCondicoesMedicas()
        {
            const string key = "condicoes-medicas-key";

            try
            {
                var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

                if (response is null)
                {
                    response = await _condicaoMedicaService.ConsultarCondicoesMedicas();

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
        public async Task<IActionResult> IncluirCondicaoMedica([FromBody] string Nome)
        {
            try
            {
                await _condicaoMedicaService.IncluirCondicaoMedica(Nome);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirInstrumento([FromQuery] int Id)
        {
            try
            {
                await _condicaoMedicaService.ExcluirCondicaoMedica(Id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarInstrumento([FromQuery] int Id, [FromBody] string NovoNome)
        {
            try
            {
                await _condicaoMedicaService.EditarCondicaoMedica(Id, NovoNome);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
