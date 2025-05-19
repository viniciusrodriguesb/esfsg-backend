using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class RegiaoService : IRegiaoService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public RegiaoService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<TabelaDominioResponse>> ConsultarRegioes()
        {
            return await _context.REGIAO
                                 .AsNoTracking()
                                 .Select(x => new TabelaDominioResponse()
                                 {
                                     Id = x.Id,
                                     Descricao = x.Nome
                                 }).ToListAsync();
        }

    }
}
