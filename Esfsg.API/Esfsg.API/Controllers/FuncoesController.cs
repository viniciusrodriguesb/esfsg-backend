using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Consulta todas as funções disponíveis na igreja.")]
        public async Task<IActionResult> ConsultarFuncoesIgreja()
        {
            const string key = "funcoes-igreja-key";

            var response = _memoryCacheService.Get<List<TabelaDominioResponse>>(key);

            if (response is null || _env.IsDevelopment())
            {
                response = await _funcoesService.ConsultarFuncoesIgreja();

                if (response == null || !response.Any())
                    return NotFound("Nenhum registro encontrado.");

                _memoryCacheService.Set(key, response, TimeSpan.FromMinutes(60));
            }

            return Ok(response);
        }

        [HttpGet("evento")]
        [SwaggerOperation(Summary = "Consulta todas as funções disponíveis no evento.")]
        public async Task<IActionResult> ConsultarFuncoesEvento(int IdEvento)
        {
            var response = await _funcoesService.ConsultarFuncoesEvento(IdEvento);

            if (response == null || !response.Any())
                return NotFound("Nenhum registro encontrado.");

            return Ok(response);
        }

    }
}
