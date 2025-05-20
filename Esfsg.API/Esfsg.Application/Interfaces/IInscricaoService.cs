using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IInscricaoService
    {
        Task RealizarInscricao(InscricaoRequest request);
        Task<InscricaoResponse?> ConsultarInscricao(InscricaoEventoResquest request);
        Task CancelarInscricao(int Id);
    }
}
