﻿using Esfsg.Application.Interfaces;
using Hangfire;

namespace Esfsg.API.Hangfire.Jobs
{
    public class EmailReembolsoJob
    {

        #region Construtor
        private readonly IEmailStatusService _emailStatusService;
        public EmailReembolsoJob(IEmailStatusService emailStatusService)
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
                await _emailStatusService.EnviarEmailReembolso();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
