using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class StatusService : IStatusService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public StatusService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task AtualizarStatusInscricao(StatusEnum novoStatus, int IdInscricao)
        {
            var inscricaoExiste = await _context.INSCRICAO
                                          .AsNoTracking()
                                          .AnyAsync(x => x.Id == IdInscricao);

            if (!inscricaoExiste)
                throw new NotFoundException("Inscrição não encontrada");

            var statusAtual = await _context.INSCRICAO_STATUS.AsNoTracking()
                                                             .Where(x => x.InscricaoId == IdInscricao &&
                                                                          x.DhExclusao == null)
                                                             .Select(s => s.StatusId)
                                                             .FirstOrDefaultAsync();

            if (statusAtual == (int)novoStatus)
                return;

            var status = await _context.INSCRICAO_STATUS
                                       .Where(x => x.DhExclusao == null &&
                                                   x.InscricaoId == IdInscricao)
                                       .ExecuteUpdateAsync(s => s.SetProperty(d => d.DhExclusao, DateTime.UtcNow));

            var novo = new INSCRICAO_STATUS()
            {
                StatusId = (int)novoStatus,
                InscricaoId = IdInscricao,
                DhInclusao = DateTime.UtcNow
            };

            await _context.INSCRICAO_STATUS.AddAsync(novo);
            await _context.SaveChangesAsync();
        }

    }
}
