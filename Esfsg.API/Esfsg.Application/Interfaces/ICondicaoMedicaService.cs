using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface ICondicaoMedicaService
    {
        Task<List<TabelaDominioResponse>> ConsultarCondicoesMedicas();
        Task ExcluirCondicaoMedica(int Id);
        Task EditarCondicaoMedica(int Id, string NovoNome);
        Task IncluirCondicaoMedica(string Nome);
    }
}
