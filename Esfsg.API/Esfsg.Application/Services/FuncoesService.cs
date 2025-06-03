using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class FuncoesService : IFuncoesService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public FuncoesService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<TabelaDominioResponse>> ConsultarFuncoesIgreja()
        {
            var funcoes = await _context.FUNCAO_IGREJA
                                        .AsNoTracking()
                                        .Select(x => new TabelaDominioResponse()
                                        {
                                            Id = x.Id,
                                            Descricao = x.Descricao,
                                        }).ToListAsync();

            return funcoes;
        }

        public async Task<List<TabelaDominioResponse>> ConsultarFuncoesEvento(int IdEvento)
        {
            var funcoesDisponiveis = await _context.FUNCAO_EVENTO
                                                   .AsNoTracking()
                                                   .GroupJoin(_context.INSCRICAO.Where(x => x.IdEvento == IdEvento),
                                                               fe => fe.Id,
                                                               i => i.IdFuncaoEvento,
                                                               (fe, inscricoes) => new
                                                               {
                                                                   Funcao = fe,
                                                                   Total = inscricoes.Count()
                                                               })
                                                   .Where(x => x.Total < x.Funcao.Quantidade)
                                                   .Select(x => new TabelaDominioResponse()
                                                   {
                                                       Id = x.Funcao.Id,
                                                       Descricao = x.Funcao.Descricao,
                                                   })
                                                   .OrderBy(x => x.Id)
                                                   .ToListAsync();

            return funcoesDisponiveis;
        }


    }
}
