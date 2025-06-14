using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class IgrejaService : IIgrejaService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public IgrejaService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<TabelaDominioResponse>> ConsultarIgrejas()
        {
            var classes = await _context.IGREJA
                                        .AsNoTracking()
                                        .Select(x => new TabelaDominioResponse()
                                        {
                                            Id = x.Id,
                                            Descricao = x.Nome,
                                        }).ToListAsync();

            return classes;
        }

        public async Task ExcluirIgreja(int Id)
        {
            await _context.IGREJA
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

        public async Task EditarIgreja(int Id, string NovoNome)
        {

            var row = await _context.IGREJA.Where(x => x.Id == Id)
                     .ExecuteUpdateAsync(s => s.SetProperty(p => p.Nome, NovoNome));

            if (row == 0)
                throw new Exception("Erro ao atualizar o nome da igreja.");
        }

        public async Task IncluirIgreja(IgrejaRequest request)
        {
            var Classe = new IGREJA()
            {
                Nome = request.Nome,
                PastorId = request.IdPastor,
                RegiaoId = request.IdRegiao
            };

            await _context.IGREJA.AddAsync(Classe);
            await _context.SaveChangesAsync();
        }
    }
}
