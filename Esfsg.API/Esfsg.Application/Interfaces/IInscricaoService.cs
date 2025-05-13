using Esfsg.Application.DTOs.Request;

namespace Esfsg.Application.Interfaces
{
    public interface IInscricaoService
    {
        Task RealizarInscricao(InscricaoRequest request);
    }
}
