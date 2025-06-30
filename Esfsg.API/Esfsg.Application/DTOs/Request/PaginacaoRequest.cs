namespace Esfsg.Application.DTOs.Request
{
    public class PaginacaoRequest
    {
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
    }
}
