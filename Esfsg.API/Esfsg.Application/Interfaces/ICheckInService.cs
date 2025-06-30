using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface ICheckInService
    {
        Task<PaginacaoResponse<CheckInListaResponse>> Consultar(ConsultaCheckInRequest request, PaginacaoRequest paginacao);
        Task<ResultResponse<CheckinValidadoResponse>> ConfirmarPresencaPorQRCode(ValidaPresencaRequest request);
        Task<ResultResponse<List<CheckinValidadoResponse>>> ConfirmarPresencaPorId(ValidaPresencaIdRequest request);
    }
}
