using Esfsg.Application.DTOs.Request;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Filtros
{
    public static class FiltroCheckIn
    {
        public static IQueryable<CHECK_IN> AplicarFiltro(this IQueryable<CHECK_IN> query, ConsultaCheckInRequest filtro)
        {
            if (!string.IsNullOrEmpty(filtro.Nome))
                query = query.Where(x => x.IdInscricaoNavigation.IdUsuarioNavigation.NomeCompleto.Contains(filtro.Nome));

            if (!string.IsNullOrEmpty(filtro.Periodo))
                query = query.Where(x => x.IdInscricaoNavigation.Periodo.Contains(filtro.Periodo));

            if (filtro.FuncaoEvento != null)
                query = query.Where(x => filtro.FuncaoEvento.Contains(x.IdInscricaoNavigation.IdFuncaoEvento));

            return query;
        }
    }
}
