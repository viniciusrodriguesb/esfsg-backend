using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IEventoService
    {
        Task IncluirEvento(EventoRequest request);
        Task ExcluirEvento(int Id);
        Task EditarEvento(int Id, AlteraEventoRequest request);
        Task<List<EventoResponse>> ConsultarEvento(int RegiaoId);
    }
}
