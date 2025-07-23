using Esfsg.Application.DTOs.Request;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Filtros
{
    public static class FiltroGestaoUsuario
    {
        public static IQueryable<USUARIO> AplicarFiltro(this IQueryable<USUARIO> query, GestaoUsuarioRequest filtro)
        {         
            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                var nomeFiltro = filtro.Nome.Trim().ToLower();
                query = query.Where(x => x.NomeCompleto.ToLower().Contains(nomeFiltro));
            }

            if (!string.IsNullOrEmpty(filtro.Cpf))
                query = query.Where(x => x.Cpf.Contains(filtro.Cpf));            

            if (filtro.IdIgreja.HasValue)
                query = query.Where(x => x.IdIgreja == filtro.IdIgreja);

            if (filtro.IdClasse.HasValue)
                query = query.Where(x => x.IdClasse == filtro.IdClasse);

            if (filtro.TipoUsuario.HasValue)
                query = query.Where(x => x.IdTipoUsuario == filtro.TipoUsuario);

            return query;
        }
    }
}
