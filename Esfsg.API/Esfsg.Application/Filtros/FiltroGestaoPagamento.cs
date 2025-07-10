using Esfsg.Application.DTOs.Request;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Filtros
{
    public static class FiltroGestaoPagamento
    {
        public static IQueryable<PAGAMENTO> AplicarFiltro(this IQueryable<PAGAMENTO> query, ConsultaGestaoPagamentoRequest filtro)
        {
            query = query.Where(x => x.InscricaoNavigation.IdEvento == filtro.IdEvento);
            query = query.Where(x => x.InscricaoNavigation.InscricaoStatus.Any(x => x.StatusId == (int)filtro.Status && x.DhExclusao == null));

            if (!string.IsNullOrEmpty(filtro.Nome))
                query = query.Where(x => x.InscricaoNavigation.IdUsuarioNavigation.NomeCompleto.Contains(filtro.Nome));


            return query;
        }
    }
}
