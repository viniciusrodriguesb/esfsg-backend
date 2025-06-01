using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IVisitaService
    {
        Task<List<InscritosVisitaResponse>> ConsultarInscritosVisita(int IdEvento);
        List<TabelaDominioResponse> ConsultarFuncoesVisita();
        Task CriarVisita(VisitaRequest visita);
        Task AlocarInscritosVisita(List<AlocarVisitaRequest> alocacoes);
        Task ExcluirVisita(int Id);
    }
}
