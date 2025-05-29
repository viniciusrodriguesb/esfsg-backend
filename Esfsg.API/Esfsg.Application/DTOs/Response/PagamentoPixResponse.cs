namespace Esfsg.Application.DTOs.Response
{
    public class PagamentoPixResponse
    {
        public string Status { get; set; }
        public string QrCode { get; set; }
        public string PixCopiaCola { get; set; }
        public string IdTransacao { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
