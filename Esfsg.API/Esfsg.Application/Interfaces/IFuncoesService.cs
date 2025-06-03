using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IFuncoesService
    {
        Task<List<TabelaDominioResponse>> ConsultarFuncoesIgreja();
        Task<List<TabelaDominioResponse>> ConsultarFuncoesEvento(int IdEvento);
    }
}
