using Esfsg.Application.Interfaces;
using Esfsg.Hangfire.Configurations;
using Hangfire;

namespace Esfsg.Hangfire.Jobs
{
    public class EmailQrCodePagamentoJob : IJob
    {

        #region Construtor
        private readonly IEmailStatusService _emailStatusService;
        public EmailQrCodePagamentoJob(IEmailStatusService emailStatusService)
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
                await _emailStatusService.EnviarEmailQrCodePagamento();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
