using Esfsg.Application.Enums;

namespace Esfsg.Application.Interfaces
{
    public interface IStatusService
    {
        Task AtualizarStatusInscricao(StatusEnum novoStatus, int IdInscricao);
    }
}
