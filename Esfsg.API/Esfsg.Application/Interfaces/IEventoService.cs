using Esfsg.Application.DTOs.Request;

namespace Esfsg.Application.Interfaces
{
    public interface IEventoService
    {
        Task IncluirEvento(EventoRequest request);
        Task ExcluirEvento(int Id);
        Task EditarEvento(int Id, AlteraEventoRequest request);
    }
}
