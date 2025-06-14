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

        public async Task<List<EventoResponse>> ConsultarEvento(int RegiaoId)
        {
            return await _context.EVENTO
                                       .AsNoTracking()
                                       .Where(x => x.IdIgrejaEventoNavigation.RegiaoId == RegiaoId)
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
                                       }).ToListAsync();
        }

        public async Task<List<string>?> ConsultarPeriodos(int IdEvento)
        {
            const int TotalIntegral = 50;
            const int TotalTarde = 100;

            var inscritosPorPeriodo = await _context.INSCRICAO
                .AsNoTracking()
                .Where(x => x.IdEvento == IdEvento &&
                            (x.Periodo.ToLower() == "integral" || x.Periodo.ToLower() == "tarde"))
                .GroupBy(x => x.Periodo)
                .Select(g => new { Periodo = g.Key, Quantidade = g.Count() })
                .ToListAsync();

            var result = new List<string>();

            var integral = inscritosPorPeriodo.FirstOrDefault(x => x.Periodo.Equals("Integral", StringComparison.OrdinalIgnoreCase))?.Quantidade ?? 0;
            if (integral < TotalIntegral)
                result.Add("Integral");

            var tarde = inscritosPorPeriodo.FirstOrDefault(x => x.Periodo.Equals("Tarde", StringComparison.OrdinalIgnoreCase))?.Quantidade ?? 0;
            if (tarde < TotalTarde)
                result.Add("Tarde");

            return result;
        }

        public async Task IncluirEvento(EventoRequest request)
        {
            await ValidarEvento(request);

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
        }

        public async Task ExcluirEvento(int Id)
        {
            await _context.EVENTO
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

        public async Task EditarEvento(int Id, AlteraEventoRequest request)
        {
            var evento = await _context.EVENTO
                                       .Where(x => x.Id == Id)
                                       .FirstOrDefaultAsync();

            if (evento == null)
                throw new ArgumentException("Evento não localizado.");

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
        }

        #region Métodos Privados
        private async Task ValidarEvento(EventoRequest request)
        {
            var evento = await _context.EVENTO
                                       .AsNoTracking()
                                       .Where(x => x.DhEvento.Date == request.DhEvento.Date &&
                                                   x.IdIgrejaEvento == request.IdIgrejaEvento)
                                       .AnyAsync();

            if (evento)
                throw new ArgumentException("Evento já existente na data e igreja selecionados.");
        }
        #endregion
    }
}
