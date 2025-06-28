using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponse?> ConsultarDadosDashboard(int IdEvento);
        Task<EventoProximoResponse?> ConsultarEventoProximo();
    }
}
