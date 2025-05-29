using Esfsg.Application.DTOs.Response;

namespace Esfsg.Application.Interfaces
{
    public interface IQrCodeService
    {
        Task<QRCodeResponse> GerarQRCodeAcesso(int IdInscricao);
        Task<QrCodePagamentoResponse> ObterQrCodePagamento(int IdInscricao);
    }
}
