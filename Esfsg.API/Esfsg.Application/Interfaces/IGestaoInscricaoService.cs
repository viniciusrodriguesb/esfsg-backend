using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IGestaoInscricaoService
    {
        Task<PaginacaoResponse<InscricaoParaLiberacaoResponse>> ConsultarInscricoesParaLiberacao(InscricoesPendentesRequest request, PaginacaoRequest paginacao);
        Task<PaginacaoResponse<GestaoInscricaoResponse>> ConsultarInscricoes(FiltroGestaoInscricaoRequest filtro, PaginacaoRequest paginacao);
    }
}
