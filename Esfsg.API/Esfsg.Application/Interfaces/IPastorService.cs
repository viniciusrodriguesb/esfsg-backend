using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IPastorService
    {
        Task<List<TabelaDominioResponse>> Consultar();
        Task Excluir(int Id);
        Task Editar(int Id, string NovoNome);
        Task Incluir(string Nome);
    }
}
