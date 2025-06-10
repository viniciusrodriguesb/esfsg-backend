using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Filtros;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Esfsg.Application.Services
{
    public class CheckInService : ICheckInService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public CheckInService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<CheckInListaResponse>> Consultar(ConsultaCheckInRequest request)
        {
            var query = _context.CHECK_IN
                                 .AsNoTracking()
                                 .Include(i => i.IdInscricaoNavigation)
                                    .ThenInclude(u => u.IdUsuarioNavigation)
                                        .ThenInclude(c => c.IdClasseNavigation)
                                 .Include(i => i.IdInscricaoNavigation)
                                    .ThenInclude(f => f.IdFuncaoEventoNavigation)
                                 .AsQueryable();

            var result = await query.AplicarFiltro(request)
                                    .Select(x => new CheckInListaResponse()
                                    {
                                        IdCheckin = x.Id,
                                        Presenca = x.Presente,
                                        Nome = x.IdInscricaoNavigation.IdUsuarioNavigation.NomeCompleto,
                                        Evento = new DadosEventoResponse()
                                        {
                                            Periodo = x.IdInscricaoNavigation.Periodo,
                                            FuncaoEvento = x.IdInscricaoNavigation.IdFuncaoEventoNavigation.Descricao
                                        },
                                        Igreja = new DadosIgrejaResponse()
                                        {
                                            Igreja = x.IdInscricaoNavigation.IdUsuarioNavigation.IdIgrejaNavigation.Nome,
                                            Classe = x.IdInscricaoNavigation.IdUsuarioNavigation.IdClasseNavigation.Descricao
                                        }
                                    }).ToListAsync();

            return result;
        }


        public async Task<ResultResponse<CheckinValidadoResponse>> ConfirmarPresenca(ValidaPresencaRequest request)
        {
            int Id = ValidarEntradaDados(request);

            var result = await _context.CHECK_IN.Where(x => x.Id == Id)
                                                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Presente, request.Presenca));

            if (result == 0)
            {
                return new ResultResponse<CheckinValidadoResponse>()
                {
                    Mensagem = "Erro ao confirmar ou zerar a presença.",
                    Sucesso = false
                };
            }

            var response = await ConsultarDadosCheckin(Id);

            return new ResultResponse<CheckinValidadoResponse>()
            {
                Mensagem = request.Presenca ? "Presença confirmada com sucesso." : "Presença zerada com sucesso.",
                Sucesso = true,
                Dados = response
            };
        }

        #region Métodos Privados
        private static int ValidarEntradaDados(ValidaPresencaRequest request)
        {
            if (request.IdCheckIn.HasValue)
                return request.IdCheckIn.Value;
            else if (!string.IsNullOrEmpty(request.QrCode))
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(request.QrCode));
                return int.Parse(decoded);
            }
            else
                throw new ArgumentException("Para validar é necessário o ID ou o QRCode.");
        }

        private async Task<CheckinValidadoResponse?> ConsultarDadosCheckin(int Id)
        {
            return await _context.CHECK_IN
                                         .AsNoTracking()
                                         .Where(x => x.Id == Id)
                                         .Include(i => i.IdInscricaoNavigation)
                                            .ThenInclude(u => u.IdUsuarioNavigation)
                                          .Include(i => i.IdInscricaoNavigation)
                                             .ThenInclude(u => u.IdFuncaoEventoNavigation)
                                          .Include(i => i.IdInscricaoNavigation)
                                             .ThenInclude(vp => vp.VisitaParticipantes)
                                                .ThenInclude(v => v.IdVisitaNavigation)
                                         .Select(x => new CheckinValidadoResponse()
                                         {
                                             NomeCompleto = x.IdInscricaoNavigation.IdUsuarioNavigation.NomeCompleto,
                                             Periodo = x.IdInscricaoNavigation.Periodo,
                                             Grupo = x.IdInscricaoNavigation.IdFuncaoEventoNavigation.Descricao,
                                             Pulseira = x.IdInscricaoNavigation.IdFuncaoEventoNavigation.Cor,
                                             EtiquetaVisita = x.IdInscricaoNavigation.VisitaParticipantes.FirstOrDefault().IdVisitaNavigation.CorVoluntario
                                         }).FirstOrDefaultAsync();
        }
        #endregion

    }
}
