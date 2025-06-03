using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class VisitaService : IVisitaService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public VisitaService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<InscritosVisitaResponse>> ConsultarInscritosVisita(int IdEvento)
        {
            var inscritosVisita = await _context.VISITA_PARTICIPANTE
                                                .AsNoTracking()
                                                .Where(x => x.IdInscricaoNavigation.IdEvento == IdEvento)
                                                .Select(x => new InscritosVisitaResponse()
                                                {
                                                    Nome = x.IdInscricaoNavigation.IdUsuarioNavigation.NomeCompleto,
                                                    FuncaoEvento = x.IdInscricaoNavigation.IdFuncaoEventoNavigation.Descricao,
                                                    DadosVisita = new DadosInscritoVisita()
                                                    {
                                                        Funcao = x.Funcao,
                                                        NomeVisita = x.IdVisitaNavigation != null ? x.IdVisitaNavigation.Descricao : null,
                                                        VagasCarro = x.Vagas,
                                                        Alocado = x.IdVisita != null
                                                    }
                                                }).ToListAsync();

            return inscritosVisita;
        }

        public async Task AlocarInscritosVisita(List<AlocarVisitaRequest> alocacoes)
        {

            foreach (var alocacao in alocacoes)
            {
                var jaAlocado = await _context.VISITA_PARTICIPANTE
                                              .AsNoTracking()
                                              .Include(i => i.IdInscricaoNavigation)
                                                   .ThenInclude(u => u.IdUsuarioNavigation)
                                              .FirstOrDefaultAsync(x => x.IdInscricao == alocacao.IdInscricao &&
                                                             x.IdVisita == alocacao.IdVisita);

                if (jaAlocado != null)
                    throw new ArgumentException($"O participante {jaAlocado.IdInscricaoNavigation?.IdUsuarioNavigation.NomeCompleto} já está alocado nesta visita!");

                var participante = await _context.VISITA_PARTICIPANTE.FirstOrDefaultAsync(x => x.IdInscricao == alocacao.IdInscricao);

                if (participante == null)
                    continue;

                participante.Funcao = alocacao.Funcao.ToString();
                participante.IdVisita = alocacao.IdVisita;

                _context.VISITA_PARTICIPANTE.Update(participante);
            }

            await _context.SaveChangesAsync();
        }

        public List<TabelaDominioResponse> ConsultarFuncoesVisita()
        {
            return Enum.GetValues(typeof(FuncaoVisitaEnum))
                       .Cast<FuncaoVisitaEnum>()
                       .Select(e => new TabelaDominioResponse
                       {
                           Id = (int)e,
                           Descricao = e.ToString()
                       }).ToList();
        }

        public async Task CriarVisita(VisitaRequest visita)
        {

            var existeVisita = await _context.VISITA
                                       .AsNoTracking()
                                       .Where(x => x.CorVoluntario == visita.Cor ||
                                                   x.Descricao == visita.Descricao ||
                                                   x.EnderecoVisitado == visita.Endereco)
                                       .AnyAsync();

            if (existeVisita)
                throw new ArgumentException("Já existe visita com informações enviadas.");

            var novaVisita = new VISITA()
            {
                Descricao = visita.Descricao,
                CorVoluntario = visita.Cor,
                EnderecoVisitado = visita.Endereco,
                Observacoes = visita.Observacoes
            };

            await _context.VISITA.AddAsync(novaVisita);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirVisita(int Id)
        {
            await _context.VISITA
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

    }
}
