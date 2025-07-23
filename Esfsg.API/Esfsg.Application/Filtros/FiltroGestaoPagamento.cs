using Esfsg.Application.DTOs.Request;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Filtros
{
    public static class FiltroGestaoPagamento
    {
        public static IQueryable<INSCRICAO> AplicarFiltroPagamento(this IQueryable<INSCRICAO> query, ConsultaGestaoPagamentoRequest filtro)
        {
            query = query.Where(x => x.IdEvento == filtro.IdEvento);
            query = query.Where(x => x.InscricaoStatus.Any(x => x.StatusId == (int)filtro.Status && x.DhExclusao == null));

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                var nomeFiltro = filtro.Nome.Trim().ToLower();
                query = query.Where(x => x.IdUsuarioNavigation.NomeCompleto.ToLower().Contains(nomeFiltro));
            }             

            return query;
        }
    }
}
