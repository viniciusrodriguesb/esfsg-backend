using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
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

        public async Task EditarRegiao(int Id, string NovoNome)
        {
            var row = await _context.REGIAO.Where(x => x.Id == Id)
                      .ExecuteUpdateAsync(s => s.SetProperty(p => p.Nome, NovoNome));

            if (row == 0)
                throw new Exception("Erro ao atualizar a classe.");
        }

        public async Task ExcluirRegiao(int Id)
        {
            await _context.REGIAO
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

        public async Task IncluirRegiao(string Nome)
        {
            var regiao = new REGIAO()
            {
                Nome = Nome
            };

            await _context.REGIAO.AddAsync(regiao);
            await _context.SaveChangesAsync();
        }
    }
}
