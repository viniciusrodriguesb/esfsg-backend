using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Interfaces
{
    public interface IEventoService
    {
        Task<ResultResponse<EVENTO>> IncluirEvento(EventoRequest request);
        Task ExcluirEvento(int Id);
        Task<ResultResponse<EVENTO>> EditarEvento(int Id, AlteraEventoRequest request);
        Task<List<EventoResponse>> ConsultarEvento();
        Task<List<string>?> ConsultarPeriodos(int IdEvento);
    }
}
