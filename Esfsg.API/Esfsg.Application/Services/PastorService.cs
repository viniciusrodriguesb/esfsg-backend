using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class PastorService : IPastorService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public PastorService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<TabelaDominioResponse>> Consultar()
        {
            return await _context.PASTOR.AsNoTracking()
                                        .Select(x => new TabelaDominioResponse()
                                        {
                                            Descricao = x.Nome,
                                            Id = x.Id
                                        }).OrderBy(x => x.Descricao)
                                          .ToListAsync();
        }

        public async Task Excluir(int Id)
        {
            await _context.PASTOR
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

        public async Task Editar(int Id, string NovoNome)
        {
            await _context.PASTOR
                          .Where(x => x.Id == Id)
                          .ExecuteUpdateAsync(s => s.SetProperty(p => p.Nome, NovoNome));
        }

        public async Task Incluir(string Nome)
        {
            var pastor = new PASTOR()
            {
                Nome = Nome
            };

            await _context.PASTOR.AddAsync(pastor);
            await _context.SaveChangesAsync();
        }
    }
}
