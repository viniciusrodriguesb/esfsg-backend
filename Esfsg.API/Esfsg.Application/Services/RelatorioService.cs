using Esfsg.Application.DTOs.Request.Relatorios;
using Esfsg.Application.DTOs.Response.Relatorios;
using Esfsg.Application.Enums;
using Esfsg.Application.Helpers;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class RelatorioService : IRelatorioService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public RelatorioService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<byte[]> GerarRelatorioInscricoes(RelatorioInscricaoRequest request)
        {
            var query = await _context.INSCRICAO
                                   .AsNoTracking()
                                   .Where(x => x.InscricaoStatus.Any(s => s.DhExclusao == null) &&
                                               x.IdEvento == request.IdEvento)
                                   .Include(u => u.IdUsuarioNavigation)
                                        .ThenInclude(c => c.IdClasseNavigation)
                                   .Include(u => u.IdUsuarioNavigation)
                                        .ThenInclude(c => c.IdIgrejaNavigation)
                                   .Include(fe => fe.IdFuncaoEventoNavigation)
                                   .Include(s => s.InscricaoStatus)
                                        .ThenInclude(s => s.StatusNavigation)
                                   .Include(c => c.CheckIns)
                                   .OrderBy(x => x.IdUsuarioNavigation.IdIgreja)
                                        .ThenBy(x => x.IdUsuarioNavigation.NomeCompleto)
                                   .Select(x => new RelatorioInscricaoResponse()
                                   {
                                       NomeCompleto = x.IdUsuarioNavigation.NomeCompleto,
                                       Cpf = FormatoHelper.FormatarCpf(x.IdUsuarioNavigation.Cpf),
                                       Telefone = FormatoHelper.FormatarTelefone(x.IdUsuarioNavigation.Telefone),
                                       Classe = x.IdUsuarioNavigation.IdClasseNavigation.Descricao,
                                       Igreja = x.IdUsuarioNavigation.IdIgrejaNavigation.Nome,
                                       Periodo = x.Periodo,
                                       FuncaoEvento = x.IdFuncaoEventoNavigation.Descricao,
                                       DhInscricao = x.DhInscricao.ToString("dd/MM/yyyy HH:mm:ss"),
                                       StatusInscricao = x.InscricaoStatus.Select(s => s.StatusNavigation.Descricao).FirstOrDefault(),
                                       Presenca = x.CheckIns.Select(c => c.Presente).FirstOrDefault() ? "Presente" : "Ausente"
                                   }).ToListAsync();

            const string titulo = "Relatório Inscrições";
            return request.TipoRelatorio switch
            {
                ETipoRelatorio.EXCEL => ExcelHelper<RelatorioInscricaoResponse>.ExportarParaExcel(query, titulo),
                ETipoRelatorio.PDF => PdfHelper<RelatorioInscricaoResponse>.ExportarParaPdf(query, titulo),
                _ => throw new ArgumentException("Tipo de relatório não suportado."),
            };
        }

    }
}
