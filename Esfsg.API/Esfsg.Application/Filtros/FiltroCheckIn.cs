using Esfsg.Application.DTOs.Request;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Filtros
{
    public static class FiltroCheckIn
    {
        public static IQueryable<CHECK_IN> AplicarFiltro(this IQueryable<CHECK_IN> query, ConsultaCheckInRequest filtro)
        {
            query = query.Where(x => x.IdInscricaoNavigation.IdEvento == filtro.IdEvento);

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                var nomeFiltro = filtro.Nome.Trim().ToLower();
                query = query.Where(x => x.IdInscricaoNavigation.IdUsuarioNavigation.NomeCompleto.ToLower().Contains(nomeFiltro));
            }

            if (!string.IsNullOrEmpty(filtro.Periodo))
                query = query.Where(x => x.IdInscricaoNavigation.Periodo.Contains(filtro.Periodo));

            if (filtro.FuncaoEvento != null)
                query = query.Where(x => x.IdInscricaoNavigation.IdFuncaoEvento == filtro.FuncaoEvento);

            if (filtro.Validado.HasValue)
                query = query.Where(x => x.Presente == filtro.Validado);

            return query;
        }
    }
}
