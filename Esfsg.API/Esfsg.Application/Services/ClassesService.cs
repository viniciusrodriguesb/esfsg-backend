using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class ClassesService : IClasseService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public ClassesService(DbContextBase context)
        {
            _context = context;
        } 
        #endregion

        public async Task<List<TabelaDominioResponse>> ConsultarClasses()
        {
            var classes = await _context.CLASSE
                                        .AsNoTracking()
                                        .Select(x => new TabelaDominioResponse()
                                        {
                                            Id = x.Id,
                                            Descricao = x.Descricao,
                                        }).ToListAsync();

            return classes;
        }



    }
}
