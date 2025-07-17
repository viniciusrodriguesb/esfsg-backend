using Esfsg.Application.DTOs.Request.Relatorios;
using Esfsg.Application.Enums;
using Esfsg.Application.Helpers;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class RelatorioController : ControllerBase
    {

        #region Construtor
        private readonly IRelatorioService _relatorioService;
        public RelatorioController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }
        #endregion

        [HttpGet("inscricoes")]
        public async Task<IActionResult> GerarRelatorioInscricoes([FromQuery] RelatorioInscricaoRequest request)
        {
            string contentType;
            string nomeArquivo;
            try
            {
                var arquivo = await _relatorioService.GerarRelatorioInscricoes(request);
                switch (request.TipoRelatorio)
                {
                    case ETipoRelatorio.EXCEL:
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        nomeArquivo = "Inscricoes.xlsx";
                        break;

                    case ETipoRelatorio.PDF:
                        contentType = "application/pdf";
                        nomeArquivo = "Inscricoes.pdf";
                        break;

                    default:
                        return BadRequest("Tipo inválido. Use 'excel' ou 'pdf'.");
                }

                return File(arquivo, contentType, nomeArquivo);
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
