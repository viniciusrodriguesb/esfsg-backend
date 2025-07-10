using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class CondicaoMedicaService : ICondicaoMedicaService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public CondicaoMedicaService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<TabelaDominioResponse>> ConsultarCondicoesMedicas()
        {
            return await _context.CONDICAO_MEDICA
                                 .AsNoTracking()
                                 .Select(x => new TabelaDominioResponse()
                                 {
                                     Descricao = x.Descricao,
                                     Id = x.Id
                                 }).OrderBy(x => x.Descricao)
                                   .ToListAsync();
        }

        public async Task ExcluirCondicaoMedica(int Id)
        {
            await _context.CONDICAO_MEDICA
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

        public async Task EditarCondicaoMedica(int Id, string NovoNome)
        {
            await VerificarExistencia(Id);

            await VerificarNome(NovoNome);

            var row = await _context.CONDICAO_MEDICA.Where(x => x.Id == Id)
                     .ExecuteUpdateAsync(s => s.SetProperty(p => p.Descricao, NovoNome));

            if (row == 0)
                throw new Exception("Erro ao atualizar a condição médica.");
        }

        public async Task IncluirCondicaoMedica(string Nome)
        {
            await VerificarNome(Nome);

            var condicaoMedica = new CONDICAO_MEDICA()
            {
                Descricao = Nome
            };

            await _context.CONDICAO_MEDICA.AddAsync(condicaoMedica);
            await _context.SaveChangesAsync();
        }

        #region Métodos Privados
        private async Task VerificarNome(string NovoNome)
        {
            var nome = NovoNome.Trim().ToLowerInvariant();

            var existe = await _context.CONDICAO_MEDICA.AsNoTracking()
                                                       .AnyAsync(x => x.Descricao.ToLower() == nome);

            if (existe)
                throw new InvalidOperationException("Condição médica já existente.");
        }

        private async Task VerificarExistencia(int Id)
        {
            var existe = await _context.CONDICAO_MEDICA.AsNoTracking()
                                                        .AnyAsync(x => x.Id == Id);

            if (!existe)
                throw new KeyNotFoundException("Não foi possivel atualizar. Condição médica não existe.");
        } 
        #endregion

    }
}
