using Esfsg.Application.DTOs.Request.Relatorios;
using Esfsg.Application.DTOs.Response.Relatorios;
using Esfsg.Application.Enums;
using Esfsg.Application.Helpers;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

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
                                   .Where(x => x.IdEvento == request.IdEvento)
                                   .OrderBy(x => x.IdUsuarioNavigation.IdIgreja)
                                        .ThenBy(x => x.IdUsuarioNavigation.NomeCompleto)
                                   .Select(x => new RelatorioInscricaoResponse()
                                   {
                                       NomeCompleto = x.IdUsuarioNavigation.NomeCompleto,
                                       Cpf = FormatoHelper.FormatarCpf(x.IdUsuarioNavigation.Cpf),
                                       Telefone = string.IsNullOrWhiteSpace(x.IdUsuarioNavigation.Telefone) ? string.Empty : FormatoHelper.FormatarTelefone(x.IdUsuarioNavigation.Telefone),
                                       Classe = x.IdUsuarioNavigation.IdClasseNavigation!.Descricao,
                                       Igreja = x.IdUsuarioNavigation.IdIgrejaNavigation!.Nome,
                                       Periodo = x.Periodo,
                                       FuncaoEvento = x.IdFuncaoEventoNavigation.Descricao,
                                       DhInscricao = x.DhInscricao.ToString("dd/MM/yyyy HH:mm:ss"),
                                       StatusInscricao = x.InscricaoStatus.Where(x => x.DhExclusao == null).Select(s => s.StatusNavigation.Descricao).FirstOrDefault() ?? string.Empty,
                                       Presenca = x.CheckIns.Select(c => c.Presente).FirstOrDefault() ? "Presente" : "Ausente"
                                   }).ToListAsync();

            const string titulo = "Relatório Inscrições";
            return request.TipoRelatorio switch
            {
                ETipoRelatorio.EXCEL => ExcelHelper<RelatorioInscricaoResponse>.ExportarParaExcel(query, titulo),
                ETipoRelatorio.PDF => PdfHelper<RelatorioInscricaoResponse>.ExportarParaPdf(query, titulo),
                _ => throw new BusinessException("Tipo de relatório não suportado."),
            };
        }

        public async Task<byte[]> GerarRelatorioPorFuncao(int IdEvento)
        {

            var funcoes = await _context.FUNCAO_EVENTO
                                        .AsNoTracking()
                                        .ToListAsync();

            var arquivosPdf = new List<(string NomeArquivo, byte[] ConteudoArquivo)>();
            foreach (var funcao in funcoes)
            {
                var inscricoes = await _context.INSCRICAO.AsNoTracking()
                                                   .Where(x => x.IdEvento == IdEvento &&
                                                               x.IdFuncaoEvento == funcao.Id)
                                                   .OrderBy(x => x.IdUsuarioNavigation.IdIgreja)
                                                     .ThenBy(x => x.IdUsuarioNavigation.NomeCompleto)
                                                   .Select(x => new RelatorioInscricaoResponse()
                                                   {
                                                       NomeCompleto = x.IdUsuarioNavigation.NomeCompleto,
                                                       Cpf = FormatoHelper.FormatarCpf(x.IdUsuarioNavigation.Cpf),
                                                       Telefone = string.IsNullOrWhiteSpace(x.IdUsuarioNavigation.Telefone) ? string.Empty : FormatoHelper.FormatarTelefone(x.IdUsuarioNavigation.Telefone),
                                                       Classe = x.IdUsuarioNavigation.IdClasseNavigation!.Descricao,
                                                       Igreja = x.IdUsuarioNavigation.IdIgrejaNavigation!.Nome,
                                                       Periodo = x.Periodo,
                                                       FuncaoEvento = x.IdFuncaoEventoNavigation.Descricao,
                                                       DhInscricao = x.DhInscricao.ToString("dd/MM/yyyy HH:mm:ss"),
                                                       StatusInscricao = x.InscricaoStatus.Where(x => x.DhExclusao == null).Select(s => s.StatusNavigation.Descricao).FirstOrDefault() ?? string.Empty,
                                                       Presenca = x.CheckIns.Select(c => c.Presente).FirstOrDefault() ? "Presente" : "Ausente"
                                                   }).ToListAsync();

                if (!inscricoes.Any())
                    continue;

                string titulo = $"Relatório de funções: {funcao.Descricao}";
                var nomeArquivo = $"{funcao.Descricao}.pdf";
                var pdf = PdfHelper<RelatorioInscricaoResponse>.ExportarParaPdf(inscricoes, titulo);

                arquivosPdf.Add((nomeArquivo, pdf));
            }

            using var zipStream = new MemoryStream();
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var arquivo in arquivosPdf)
                {
                    var entry = archive.CreateEntry(arquivo.NomeArquivo);

                    using var entryStream = entry.Open();
                    await entryStream.WriteAsync(arquivo.ConteudoArquivo, 0, arquivo.ConteudoArquivo.Length);
                }
            }

            zipStream.Position = 0;
            return zipStream.ToArray();
        }

    }
}
