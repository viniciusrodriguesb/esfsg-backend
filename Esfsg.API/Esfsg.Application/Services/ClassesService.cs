using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
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

        public async Task ExcluirClasse(int Id)
        {
            await _context.CLASSE
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

        public async Task EditarClasse(int Id, string NovoNome)
        {

            var row = await _context.CLASSE.Where(x => x.Id == Id)
                     .ExecuteUpdateAsync(s => s.SetProperty(p => p.Descricao, NovoNome));

            if (row == 0)
                throw new Exception("Erro ao atualizar a classe.");
        }

        public async Task IncluirClasse(string Nome)
        {
            var Classe = new CLASSE()
            {
                Descricao = Nome
            };

            await _context.CLASSE.AddAsync(Classe);
            await _context.SaveChangesAsync();
        }

    }
}
