using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IGestaoInscricaoService
    {
        Task<PaginacaoResponse<InscricaoParaLiberacaoResponse>> ConsultarInscricoesParaLiberacao(string Cpf, PaginacaoRequest paginacao);
        Task<PaginacaoResponse<GestaoInscricaoResponse>> ConsultarInscricoes(FiltroGestaoInscricaoRequest filtro, PaginacaoRequest paginacao);
    }
}
