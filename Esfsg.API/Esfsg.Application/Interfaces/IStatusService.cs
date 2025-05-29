using Esfsg.Application.Enum;

namespace Esfsg.Application.Interfaces
{
    public interface IStatusService
    {
        Task AtualizarStatusInscricao(StatusEnum novoStatus, int IdInscricao);
    }
}
