using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IGestaoInscricaoService
    {
        Task<List<InscricaoParaLiberacaoResponse>?> ConsultarInscricoesParaLiberacao(string Cpf);
        Task<List<GestaoInscricaoResponse>?> ConsultarInscricoes(FiltroGestaoInscricaoRequest filtro);
    }
}
