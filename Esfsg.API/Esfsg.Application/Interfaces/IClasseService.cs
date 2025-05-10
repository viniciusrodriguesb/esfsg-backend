using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IClasseService
    {
        Task<List<TabelaDominioResponse>> ConsultarClasses();
    }
}
