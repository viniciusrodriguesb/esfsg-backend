using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IInstrumentoService
    {
        Task<List<TabelaDominioResponse>> ConsultarInstrumentos();
        Task ExcluirInstrumento(int Id);
        Task EditarInstrumento(int Id, string NovoNome);
        Task IncluirInstrumento(string Nome);
    }
}
