using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class EventoService : IEventoService
    {
        #region Construtor
        private readonly DbContextBase _context;
        public EventoService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<EventoResponse>> ConsultarEvento()
        {
            return await _context.EVENTO.AsNoTracking()
                                        .Where(x => x.Ativo)
                                        .Select(e => new EventoResponse()
                                        {
                                            Id = e.Id,
                                            Nome = e.Nome,
                                            DataEvento = e.DhEvento.ToString("dd/MM/yyyy"),
                                            LimiteIntegral = e.LimiteIntegral,
                                            LimiteParcial = e.LimiteParcial,
                                            LinkGrupoWpp = new Uri(e.LinkWpp),
                                            ValorIntegral = e.ValorIntegral,
                                            ValorParcial = e.ValorParcial,
                                            IgrejaEvento = e.IdIgrejaEventoNavigation.Nome,
                                            IgrejaVigilia = e.IdIgrejaVigiliaNavigation.Nome,
                                            Regiao = e.IdIgrejaEventoNavigation.RegiaoNavigation.Nome
                                        }).ToListAsync();
        }

        public async Task<List<string>?> ConsultarPeriodos(int IdEvento)
        {
            var inscritosPorPeriodo = await _context.INSCRICAO
                .AsNoTracking()
                .Where(x => x.IdEvento == IdEvento &&
                            (x.Periodo.ToLower() == "integral" || x.Periodo.ToLower() == "tarde"))
                .GroupBy(x => new { x.IdEventoNavigation.LimiteIntegral, x.IdEventoNavigation.LimiteParcial })
                .Select(g => new
                {
                    LimiteIntegral = g.Key.LimiteIntegral,
                    LimiteParcial = g.Key.LimiteParcial,
                    QuantidadeIntegral = g.Count(x => x.Periodo.ToLower() == "integral"),
                    QuantidadeParcial = g.Count(x => x.Periodo.ToLower() == "tarde")
                }).FirstOrDefaultAsync();

            var result = new List<string> { "Integral", "Tarde" };

            if (inscritosPorPeriodo == null) return result;

            if (inscritosPorPeriodo.QuantidadeIntegral >= inscritosPorPeriodo.LimiteIntegral)
                result.Remove("Integral");

            if (inscritosPorPeriodo.QuantidadeParcial >= inscritosPorPeriodo.LimiteParcial)
                result.Remove("Tarde");

            return result;
        }

        public async Task<ResultResponse<EVENTO>> IncluirEvento(EventoRequest request)
        {
            if (await VerificarEventoExistente(request))
            {
                return new ResultResponse<EVENTO>()
                {
                    Sucesso = false,
                    Mensagem = "Evento já existente na data e local solicitado."
                };
            }

            var novoEvento = new EVENTO()
            {
                DhEvento = request.DhEvento,
                IdIgrejaEvento = request.IdIgrejaEvento,
                IdIgrejaVigilia = request.IdIgrejaVigilia,
                LimiteIntegral = request.LimiteInscricoesIntegral,
                LimiteParcial = request.LimiteInscricoesParcial,
                ValorIntegral = request.ValorInscricaoIntegral,
                ValorParcial = request.ValorInscricaoParcial,
                LinkWpp = request.LinkGrupoWpp,
                Nome = request.Nome
            };

            await _context.EVENTO.AddAsync(novoEvento);
            await _context.SaveChangesAsync();

            return new ResultResponse<EVENTO>()
            {
                Sucesso = true,
                Mensagem = "Evento criado com sucesso.",
                Dados = novoEvento
            };
        }

        public async Task ExcluirEvento(int Id)
        {
            await _context.EVENTO
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

        public async Task<ResultResponse<EVENTO>> EditarEvento(int Id, AlteraEventoRequest request)
        {
            var evento = await _context.EVENTO
                                       .Where(x => x.Id == Id)
                                       .FirstOrDefaultAsync();

            if (evento == null)
            {
                return new ResultResponse<EVENTO>()
                {
                    Sucesso = false,
                    Mensagem = "Evento não localizado."
                };
            }

            if (!string.IsNullOrWhiteSpace(request.Nome))
                evento.Nome = request.Nome;

            if (request.LimiteInscricoesIntegral.HasValue)
                evento.LimiteIntegral = request.LimiteInscricoesIntegral.Value;

            if (request.LimiteInscricoesParcial.HasValue)
                evento.LimiteParcial = request.LimiteInscricoesParcial.Value;

            if (request.DhEvento.HasValue)
                evento.DhEvento = request.DhEvento.Value;

            if (!string.IsNullOrWhiteSpace(request.LinkGrupoWpp))
                evento.LinkWpp = request.LinkGrupoWpp;

            if (request.ValorInscricaoIntegral.HasValue)
                evento.ValorIntegral = request.ValorInscricaoIntegral.Value;

            if (request.ValorInscricaoParcial.HasValue)
                evento.ValorParcial = request.ValorInscricaoParcial.Value;

            if (request.IdIgrejaVigilia.HasValue)
                evento.IdIgrejaVigilia = request.IdIgrejaVigilia.Value;

            if (request.IdIgrejaEvento.HasValue)
                evento.IdIgrejaEvento = request.IdIgrejaEvento.Value;

            await _context.SaveChangesAsync();

            var response = new EVENTO()
            {
                Id = evento.Id,
                Nome = evento.Nome,
                DhEvento = evento.DhEvento,
                LimiteIntegral = evento.LimiteIntegral,
                LimiteParcial = evento.LimiteParcial,
                LinkWpp = evento.LinkWpp,
                ValorIntegral = evento.ValorIntegral,
                ValorParcial = evento.ValorParcial,
                IdIgrejaVigilia = evento.IdIgrejaVigilia,
                IdIgrejaEvento = evento.IdIgrejaEvento
            };

            return new ResultResponse<EVENTO>()
            {
                Sucesso = true,
                Mensagem = "Evento editado com sucesso.",
                Dados = response
            };
        }

        #region Métodos Privados
        private async Task<bool> VerificarEventoExistente(EventoRequest request)
        {
            return await _context.EVENTO
                                       .AsNoTracking()
                                       .Where(x => x.DhEvento.Date == request.DhEvento.Date &&
                                                   x.IdIgrejaEvento == request.IdIgrejaEvento)
                                       .AnyAsync();
        }
        #endregion
    }
}
