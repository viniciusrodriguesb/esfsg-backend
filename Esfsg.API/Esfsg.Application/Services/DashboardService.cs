using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Esfsg.Application.Services
{
    public class DashboardService : IDashboardService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public DashboardService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<DashboardResponse?> ConsultarDadosDashboard(int IdEvento)
        {
            var evento = await _context.EVENTO.AsNoTracking()
                                              .Where(x => x.Id == IdEvento)
                                              .Select(e => new DashboardResponse()
                                              {
                                                  Inscritos = new DadosInscritos()
                                                  {
                                                      Confirmados = new DadosQuantitativo()
                                                      {
                                                          IdStatus = (int)StatusEnum.PAGAMENTO_CONFIRMADO,
                                                          Quantidade = e.Inscricaos.Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.PAGAMENTO_CONFIRMADO && s.DhExclusao == null)).Count(),
                                                          Percentual = e.Inscricaos.Any() ?
                                                                       ((decimal)e.Inscricaos.Count(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.PAGAMENTO_CONFIRMADO && s.DhExclusao == null)) / e.Inscricaos.Count()) * 100
                                                                       : 0
                                                      },
                                                      AguardandoLiberacao = new DadosQuantitativo()
                                                      {
                                                          IdStatus = (int)StatusEnum.AGUARDANDO_LIBERACAO,
                                                          Quantidade = e.Inscricaos.Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.AGUARDANDO_LIBERACAO && s.DhExclusao == null)).Count(),
                                                          Percentual = e.Inscricaos.Any() ?
                                                                       ((decimal)e.Inscricaos.Count(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.AGUARDANDO_LIBERACAO && s.DhExclusao == null)) / e.Inscricaos.Count()) * 100
                                                                       : 0
                                                      },
                                                      Pendentes = new DadosQuantitativo()
                                                      {
                                                          IdStatus = (int)StatusEnum.AGUARDANDO_PAGAMENTO,
                                                          Quantidade = e.Inscricaos.Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.AGUARDANDO_PAGAMENTO && s.DhExclusao == null)).Count(),
                                                          Percentual = e.Inscricaos.Any() ?
                                                                       ((decimal)e.Inscricaos.Count(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.AGUARDANDO_PAGAMENTO && s.DhExclusao == null)) / e.Inscricaos.Count()) * 100
                                                                       : 0
                                                      },
                                                      Cancelados = new DadosQuantitativo()
                                                      {
                                                          IdStatus = (int)StatusEnum.CANCELADA,
                                                          Quantidade = e.Inscricaos.Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.CANCELADA && s.DhExclusao == null)).Count(),
                                                          Percentual = e.Inscricaos.Any() ?
                                                                       ((decimal)e.Inscricaos.Count(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.CANCELADA && s.DhExclusao == null)) / e.Inscricaos.Count()) * 100
                                                                       : 0
                                                      },
                                                      ReembolsoSolicitado = new DadosQuantitativo()
                                                      {
                                                          IdStatus = (int)StatusEnum.REEMBOLSO_SOLICITADO,
                                                          Quantidade = e.Inscricaos.Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.REEMBOLSO_SOLICITADO && s.DhExclusao == null)).Count(),
                                                          Percentual = e.Inscricaos.Any() ?
                                                                       ((decimal)e.Inscricaos.Count(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.REEMBOLSO_SOLICITADO && s.DhExclusao == null)) / e.Inscricaos.Count()) * 100
                                                                       : 0
                                                      },
                                                  },
                                                  InscritosPeriodo = new DadosInscritosPeriodo()
                                                  {
                                                      QuantidadeInscritosIntegral = e.Inscricaos.Where(x => x.Periodo.ToLower() == "integral").Count(),
                                                      QuantidadeInscritosTarde = e.Inscricaos.Where(x => x.Periodo.ToLower() == "tarde").Count(),
                                                  },
                                                  Arrecadacao = new DadosPagamento
                                                  {
                                                      ValorArrecadadoIntegral = e.Inscricaos.Where(i => i.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.PAGAMENTO_CONFIRMADO &&
                                                                                                                                   s.DhExclusao == null)).Sum(i => i.Periodo.ToLower() == "integral" ? e.ValorIntegral : 0).ToString("C", new CultureInfo("pt-BR")),
                                                      ValorArrecadadoParcial = e.Inscricaos.Where(i => i.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.PAGAMENTO_CONFIRMADO &&
                                                                                                                                   s.DhExclusao == null)).Sum(i => i.Periodo.ToLower() == "tarde" ? e.ValorParcial : 0).ToString("C", new CultureInfo("pt-BR")),
                                                      Total = e.Inscricaos.Where(i => i.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.PAGAMENTO_CONFIRMADO &&
                                                                                                                           s.DhExclusao == null))
                                                                .Sum(i => i.Periodo.ToLower() == "integral" ? e.ValorIntegral
                                                                    : i.Periodo.ToLower() == "tarde" ? e.ValorParcial : 0).ToString("C", new CultureInfo("pt-BR"))
                                                  },
                                                  InscritosVisita = new DadosVisita()
                                                  {
                                                      TotalVisitas = _context.VISITA.AsNoTracking().Count(),
                                                      InscritosAlocados = e.Inscricaos.SelectMany(v => v.VisitaParticipantes).Where(v => v.IdVisita != null).Count(),
                                                      InscritosDisponiveisVisita = e.Inscricaos.SelectMany(v => v.VisitaParticipantes).Where(v => v.IdVisita == null).Count()
                                                  }
                                              }).FirstOrDefaultAsync();

            return evento;
        }

        public async Task<EventoProximoResponse?> ConsultarEventoProximo(int IdRegiao)
        {
            return await _context.EVENTO.AsNoTracking()
                                        .Where( x => x.IdIgrejaEventoNavigation.RegiaoId == IdRegiao)
                                        .OrderBy(x => x.DhEvento)
                                        .Select(e => new EventoProximoResponse()
                                        {
                                            Id = e.Id,
                                            Data = e.DhEvento.ToString("dd/MM/yyyy"),
                                            Nome = e.Nome,
                                            Igreja = e.IdIgrejaEventoNavigation.Nome
                                        }).FirstOrDefaultAsync();
        }

    }
}
