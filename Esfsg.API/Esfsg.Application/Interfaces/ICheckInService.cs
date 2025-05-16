using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface ICheckInService
    {
        Task<List<CheckInListaResponse>> Consultar(ConsultaCheckInRequest request);
        Task ConfirmarPresenca(ValidaPresencaRequest request);
    }
}
