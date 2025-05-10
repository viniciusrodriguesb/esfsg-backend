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

        public async Task<List<TabelaDominioResponse>> ConsultarFuncoesEvento()
        {
            var funcoes = await _context.FUNCAO_EVENTO
                                        .AsNoTracking()
                                        .Select(x => new TabelaDominioResponse()
                                        {
                                            Id = x.Id,
                                            Descricao = x.Descricao,
                                        }).ToListAsync();

            return funcoes;
        }


    }
}
