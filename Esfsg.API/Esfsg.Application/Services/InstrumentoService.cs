using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class InstrumentoService: IInstrumentoService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public InstrumentoService(DbContextBase context)
        {
            _context = context;
        } 
        #endregion

        public async Task<List<TabelaDominioResponse>> ConsultarInstrumentos()
        {
            return await _context.INSTRUMENTO
                                             .AsNoTracking()
                                             .Select(x => new TabelaDominioResponse()
                                             {
                                                 Descricao = x.Descricao,
                                                 Id = x.Id
                                             }).ToListAsync();
        }

        public async Task ExcluirInstrumento(int Id)
        {
            await _context.INSTRUMENTO
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

        public async Task EditarInstrumento(int Id, string NovoNome)
        {
            await _context.INSTRUMENTO
                          .Where(x => x.Id == Id)
                          .ExecuteUpdateAsync(s => s.SetProperty(p => p.Descricao, NovoNome));
        }

        public async Task IncluirInstrumento(string Nome)
        {
            var instrumento = new INSTRUMENTO()
            {
                Descricao = Nome
            };

            await _context.INSTRUMENTO.AddAsync(instrumento);
            await _context.SaveChangesAsync();
        }



    }
}
