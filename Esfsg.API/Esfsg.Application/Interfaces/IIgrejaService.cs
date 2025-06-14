using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IIgrejaService
    {
        Task<List<TabelaDominioResponse>> ConsultarIgrejas();
        Task ExcluirIgreja(int Id);
        Task EditarIgreja(int Id, string NovoNome);
        Task IncluirIgreja(IgrejaRequest request);
    }
}
