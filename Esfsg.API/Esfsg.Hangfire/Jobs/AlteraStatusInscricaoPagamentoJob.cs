using Esfsg.Application.Interfaces;
using Esfsg.Hangfire.Configurations;
using Hangfire;

namespace Esfsg.Hangfire.Jobs
{
    public class AlteraStatusInscricaoPagamentoJob : IJob
    {

        #region Construtor
        private readonly IPagamentoService _pagamentoService;
        public AlteraStatusInscricaoPagamentoJob(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }
        #endregion

        [AutomaticRetry(Attempts = 1)]
        [DisableConcurrentExecution(10000)]
        public async Task Execute()
        {
            try
            {
                await _pagamentoService.AlterarStatusInscricao();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
