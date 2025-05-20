using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IClasseService
    {
        Task<List<TabelaDominioResponse>> ConsultarClasses();
        Task ExcluirClasse(int Id);
        Task EditarClasse(int Id, string NovoNome);
        Task IncluirClasse(string Nome);
    }
}
