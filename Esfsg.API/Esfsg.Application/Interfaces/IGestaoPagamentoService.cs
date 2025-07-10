using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IGestaoPagamentoService
    {
        Task<PaginacaoResponse<DadosGestaoPagamentoResponse>> ObterDadosPagamentoInscricao(ConsultaGestaoPagamentoRequest request, PaginacaoRequest paginacao);
        Task<ResultResponse<string>> GerarNovoCodigoPix(int IdInscricao);
    }
}
