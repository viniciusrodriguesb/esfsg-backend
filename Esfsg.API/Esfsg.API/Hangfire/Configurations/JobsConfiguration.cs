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
                  Cron.Yearly);

            jobManager.AddOrUpdate<EmailQrCodeAcessoJob>(
               "email-qrcode-pagamento",
               job => job.Execute(),
               Cron.Yearly);

            jobManager.AddOrUpdate<EmailQrCodePagamentoJob>(
               "email-qrcode-acesso",
               job => job.Execute(),
               Cron.Yearly);

            jobManager.AddOrUpdate<EmailCancelamentoJob>(
               "email-cancelamento",
               job => job.Execute(),
               Cron.Yearly);

            jobManager.AddOrUpdate<EmailReembolsoJob>(
               "email-reembolso",
               job => job.Execute(),
               Cron.Yearly);
            #endregion

            #region Pagamento
            jobManager.AddOrUpdate<GerarPagamentoJob>(
                 "gerar-pagamento",
                 job => job.Execute(),
                 Cron.Hourly);

            jobManager.AddOrUpdate<AlteraStatusInscricaoPagamentoJob>(
              "alterar-status-pagamento",
              job => job.Execute(),
              Cron.Daily); 
            #endregion

        }
    }
}
