﻿using Esfsg.Application.Interfaces;
using Esfsg.Hangfire.Configurations;
using Hangfire;

namespace Esfsg.Hangfire.Jobs
{
    public class EmailQrCodeJob : IJob
    {

        #region Construtor
        private readonly IEmailStatusService _emailStatusService;
        public EmailQrCodeJob(IEmailStatusService emailStatusService)
        {
            _emailStatusService = emailStatusService;
        }
        #endregion

        [AutomaticRetry(Attempts = 1)]
        [DisableConcurrentExecution(10000)]
        public async Task Execute()
        {
            try
            {
                await _emailStatusService.EnviarEmailQrCode();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
