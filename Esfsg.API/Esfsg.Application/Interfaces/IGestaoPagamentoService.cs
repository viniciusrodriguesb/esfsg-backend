using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IGestaoPagamentoService
    {
        Task<List<DadosGestaoPagamentoResponse>> ObterDadosPagamentoInscricao(string? Nome, int IdEvento);
        Task GerarNovoCodigoPix(int IdInscricao);
    }
}
