namespace Esfsg.Application.DTOs.Response
{
    public class PaginacaoResponse<T>
    {
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalItens { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalItens / TamanhoPagina);
        public IEnumerable<T> Itens { get; set; } = Enumerable.Empty<T>();
    }
}
