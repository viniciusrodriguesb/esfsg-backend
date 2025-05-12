using Esfsg.Application.DTOs.Request;
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
