using Esfsg.Application.DTOs.Request.Relatorios;
using Esfsg.Application.Enums;
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

        [HttpGet("funcoes")]
        public async Task<IActionResult> GerarRelatorioPorFuncao([FromQuery] int IdEvento)
        {
            var resultadoZip = await _relatorioService.GerarRelatorioPorFuncao(IdEvento);
            var nomeArquivo = $"Relatorios_Funcoes_{DateTime.UtcNow:yyyyMMdd_HHmmss}.zip";
            return File(resultadoZip, "application/zip", nomeArquivo);
        }
    }
}
