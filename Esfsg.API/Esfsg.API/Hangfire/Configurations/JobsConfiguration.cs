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

            jobManager.AddOrUpdate<EmailQrCodeJob>(
               "email-qrcode",
               job => job.Execute(),
               Cron.Yearly);

            jobManager.AddOrUpdate<EmailInscricaoConfirmadaJob>(
               "email-inscricao-confirmada",
               job => job.Execute(),
               Cron.Yearly);

            jobManager.AddOrUpdate<GerarPagamentoJob>(
              "gerar-pagamento",
              job => job.Execute(),
              Cron.Hourly);

            jobManager.AddOrUpdate<AlteraStatusInscricaoPagamentoJob>(
              "gerar-pagamento",
              job => job.Execute(),
              Cron.Daily);

        }
    }
}
