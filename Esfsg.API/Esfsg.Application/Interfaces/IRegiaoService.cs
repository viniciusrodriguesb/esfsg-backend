using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IRegiaoService
    {
        Task<List<TabelaDominioResponse>> ConsultarRegioes();
    }
}
