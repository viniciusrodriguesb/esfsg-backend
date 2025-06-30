using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Helpers
{
    public static class PaginacaoHelper
    {
        public static async Task<PaginacaoResponse<T>> PaginarDados<T>(this IQueryable<T> query, PaginacaoRequest paginacao)
        {
            var totalItens = await query.CountAsync();

            var itens = await query
            .Skip((paginacao.Pagina - 1) * paginacao.TamanhoPagina)
            .Take(paginacao.TamanhoPagina)
            .ToListAsync();

            return new PaginacaoResponse<T>
            {
                PaginaAtual = paginacao.Pagina,
                TamanhoPagina = paginacao.TamanhoPagina,
                TotalItens = totalItens,
                Itens = itens
            };
        }

    }
}
