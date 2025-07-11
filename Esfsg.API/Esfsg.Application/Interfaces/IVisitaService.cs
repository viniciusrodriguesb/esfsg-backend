using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Interfaces
{
    public interface IVisitaService
    {
        Task<PaginacaoResponse<InscritosVisitaResponse>> ConsultarInscritosVisita(ConsultaVisitaRequest request, PaginacaoRequest paginacao);
        List<TabelaDominioResponse> ConsultarFuncoesVisita();
        Task<List<VisitaResponse>> ConsultarVisitas();
        Task<ResultResponse<EditarVisitaRequest>> EditarVisitaAsync(EditarVisitaRequest request);
        Task<ResultResponse<VISITA>> CriarVisita(VisitaRequest visita);
        Task<ResultResponse<VISITA_PARTICIPANTE>> AlocarInscritosVisita(List<AlocarVisitaRequest> alocacoes);
        Task ExcluirVisita(int Id);
    }
}
