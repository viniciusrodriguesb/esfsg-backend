using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Interfaces
{
    public interface IVisitaService
    {
        Task<PaginacaoResponse<InscritosVisitaResponse>> ConsultarInscritosVisita(int IdEvento, PaginacaoRequest paginacao);
        List<TabelaDominioResponse> ConsultarFuncoesVisita();
        Task<ResultResponse<VISITA>> CriarVisita(VisitaRequest visita);
        Task<ResultResponse<VISITA_PARTICIPANTE>> AlocarInscritosVisita(List<AlocarVisitaRequest> alocacoes);
        Task ExcluirVisita(int Id);
    }
}
