using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IRegiaoService
    {
        Task<List<TabelaDominioResponse>> ConsultarRegioes();
        Task ExcluirRegiao(int Id);
        Task EditarRegiao(int Id, string NovoNome);
        Task IncluirRegiao(string Nome);
    }
}
