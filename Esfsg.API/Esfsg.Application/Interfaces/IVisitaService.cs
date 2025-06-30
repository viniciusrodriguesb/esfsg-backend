using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IVisitaService
    {
        Task<PaginacaoResponse<InscritosVisitaResponse>> ConsultarInscritosVisita(int IdEvento, PaginacaoRequest paginacao);
        List<TabelaDominioResponse> ConsultarFuncoesVisita();
        Task CriarVisita(VisitaRequest visita);
        Task AlocarInscritosVisita(List<AlocarVisitaRequest> alocacoes);
        Task ExcluirVisita(int Id);
    }
}
