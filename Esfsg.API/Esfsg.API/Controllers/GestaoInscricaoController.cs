using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Enum;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class GestaoInscricaoController : ControllerBase
    {

        #region Construtor
        private readonly IGestaoInscricaoService _gestaoInscricaoService;
        private readonly IStatusService _statusService;
        public GestaoInscricaoController(IGestaoInscricaoService gestaoInscricaoService, IStatusService statusService)
        {
            _gestaoInscricaoService = gestaoInscricaoService;
            _statusService = statusService;
        }
        #endregion

        [HttpGet("pendentes")]
        public async Task<IActionResult> ConsultarInscricoesParaLiberacao([FromQuery] string Cpf)
        {
            try
            {
                var result = await _gestaoInscricaoService.ConsultarInscricoesParaLiberacao(Cpf);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarInscricoes([FromQuery] FiltroGestaoInscricaoRequest filtro)
        {
            try
            {
                var result = await _gestaoInscricaoService.ConsultarInscricoes(filtro);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("aprovar")]
        public async Task<IActionResult> AprovarInscricao([FromBody] List<int> Ids)
        {
            try
            {
                foreach (var id in Ids)
                {
                    await _statusService.AtualizarStatusInscricao(StatusEnum.AGUARDANDO_PAGAMENTO, id);
                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
