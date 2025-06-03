using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class PastorController : ControllerBase
    {

        #region Construtor
        private readonly IPastorService _pastorService;
        public PastorController(IPastorService pastorService)
        {
            _pastorService = pastorService;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Consultar()
        {
            try
            {
                var response = await _pastorService.Consultar();

                if (response == null || !response.Any())
                    return NotFound("Nenhum registro encontrado.");

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Incluir([FromBody] string NovoNome)
        {
            try
            {
                await _pastorService.Incluir(NovoNome);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Excluir(int Id)
        {
            try
            {
                await _pastorService.Excluir(Id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromQuery] int Id, [FromBody] string NovoNome)
        {
            try
            {
                await _pastorService.Editar(Id, NovoNome);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
