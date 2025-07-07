using Esfsg.Application.DTOs.Request;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Filtros
{
    public static class FiltroVisitaParticipante
    {
        public static IQueryable<VISITA_PARTICIPANTE> AplicarFiltro(this IQueryable<VISITA_PARTICIPANTE> query, ConsultaVisitaRequest filtro)
        {
            query = query.Where(x => x.IdInscricaoNavigation.IdEvento == filtro.IdEvento);

            if (filtro.Alocado)
                query = query.Where(x => x.IdVisita != null);
            else
                query = query.Where(x => x.IdVisita == null);

            return query;
        }
    }
}
