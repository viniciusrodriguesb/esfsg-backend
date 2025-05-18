namespace Esfsg.Application.Interfaces
{
    public interface IEmailStatusService
    {
        Task EnviarEmailQrCode();
        Task EnviarEmailInscricaoRealizada();
    }
}
