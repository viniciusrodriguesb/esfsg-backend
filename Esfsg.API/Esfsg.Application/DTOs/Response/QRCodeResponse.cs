namespace Esfsg.Application.DTOs.Response
{
    public class QRCodeResponse
    {
        public int IdCheckIn { get; set; }
        public string ConteudoQrCode { get; set; } 
        public string ImagemBase64 { get; set; }
    }

    public class QrCodePagamentoResponse
    {
        public string PixCopiaCola { get; set; }
        public string ImagemBase64 { get; set; }
        public string DataExpiracao { get; set; }
    }
}
