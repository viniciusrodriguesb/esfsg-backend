namespace Esfsg.Application.Interfaces
{
    public interface IEmailStatusService
    {
        Task EnviarEmailInscricaoRealizada();
        Task EnviarEmailQrCodePagamento();
        Task EnviarEmailQrCodeAcesso();
        Task EnviarEmailCancelamentoInscricao();
        Task EnviarEmailReembolso();
    }
}
