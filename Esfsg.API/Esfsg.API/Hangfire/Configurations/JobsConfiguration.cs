using Esfsg.API.Hangfire.Jobs;
using Hangfire;

namespace Esfsg.API.Hangfire.Configurations
{
    public static class JobsConfiguration
    {
        public static void ConfigureJobs(IServiceProvider services)
        {

            using var scope = services.CreateScope();
            var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

            #region Emails
            jobManager.AddOrUpdate<EmailInscricaoConfirmadaJob>(
              "email-inscricao-confirmada",
              job => job.Execute(),
                     "*/3 * * * *");

            jobManager.AddOrUpdate<EmailQrCodePagamentoJob>(
               "email-qrcode-pagamento",
               job => job.Execute(),
                      "*/5 * * * *");

            jobManager.AddOrUpdate<EmailQrCodeAcessoJob>(
              "email-qrcode-acesso",
              job => job.Execute(),
                     "*/5 * * * *");

            jobManager.AddOrUpdate<EmailCancelamentoJob>(
               "email-cancelamento",
               job => job.Execute(),
                      "*/10 * * * *");

            jobManager.AddOrUpdate<EmailReembolsoJob>(
               "email-reembolso",
               job => job.Execute(),
                      "*/10 * * * *");
            #endregion

            #region Pagamento
            jobManager.AddOrUpdate<GerarPagamentoJob>(
                 "gerar-pagamento",
                 job => job.Execute(),
                        "*/5 * * * *");

            jobManager.AddOrUpdate<AlteraStatusInscricaoPagamentoJob>(
              "alterar-status-pagamento",
              job => job.Execute(),
                     "*/5 * * * *");
            #endregion

        }
    }
}
