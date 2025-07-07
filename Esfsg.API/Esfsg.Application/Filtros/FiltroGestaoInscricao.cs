using Esfsg.Application.DTOs.Request;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Filtros
{
    public static class FiltroGestaoInscricao
    {
        public static IQueryable<INSCRICAO> AplicarFiltro(this IQueryable<INSCRICAO> query, FiltroGestaoInscricaoRequest filtro)
        {
            query = query.Where(x => x.IdEvento == filtro.IdEvento);

            if (!string.IsNullOrEmpty(filtro.Nome))
                query = query.Where(x => x.IdUsuarioNavigation.NomeCompleto.Contains(filtro.Nome));

            if (!string.IsNullOrEmpty(filtro.Periodo))
                query = query.Where(x => x.Periodo.Contains(filtro.Periodo));

            if (filtro.FuncaoEvento.HasValue)
                query = query.Where(x => filtro.FuncaoEvento == x.IdFuncaoEvento);

            if (filtro.Igreja.HasValue)
                query = query.Where(x => x.IdUsuarioNavigation.IdIgreja == filtro.Igreja);

            if (filtro.Classe.HasValue)
                query = query.Where(x => x.IdUsuarioNavigation.IdClasse == filtro.Classe);

            if (filtro.Regiao.HasValue)
                query = query.Where(x => x.IdEventoNavigation.IdIgrejaEventoNavigation.RegiaoId == filtro.Regiao);

            return query;
        }
    }
}
