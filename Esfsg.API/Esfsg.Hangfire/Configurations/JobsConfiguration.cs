﻿using Esfsg.Hangfire.Jobs;
using Hangfire;

namespace Esfsg.Hangfire.Configurations
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

        }
    }
}
