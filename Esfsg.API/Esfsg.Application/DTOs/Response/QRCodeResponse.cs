namespace Esfsg.Application.DTOs.Response
{
    public class QRCodeResponse
    {
        public int IdCheckIn { get; set; }
        public string ConteudoQrCode { get; set; } 
        public string ImagemBase64 { get; set; }
    }
}
