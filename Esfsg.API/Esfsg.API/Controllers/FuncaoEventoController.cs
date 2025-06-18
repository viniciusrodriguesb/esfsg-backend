using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/v1")]
    public class FuncaoEventoController : ControllerBase    
    {

        #region Construtor
        private readonly IFuncoesService _funcoesService;
        public FuncaoEventoController(IFuncoesService funcoesService)
        {
            _funcoesService = funcoesService;
        }
        #endregion

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta todas as funções do evento.")]
        public async Task<IActionResult> ConsultarFuncoesEventoAdministrativo()
        {
            try
            {
                var response = await _funcoesService.ConsultarFuncoesEventoAdministrativo();
                return response == null ? NotFound("Nenhum registro encontrado.") : Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edita dados de uma função do evento.")]
        public async Task<IActionResult> ConsultarFuncoesEvento([FromBody] AlteraFuncaoEventoRequest request)
        {
            try
            {
                await _funcoesService.EditarFuncoesEvento(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
