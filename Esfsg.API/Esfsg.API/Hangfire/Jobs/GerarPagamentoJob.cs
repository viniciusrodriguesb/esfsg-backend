using Esfsg.API.Hangfire.Configurations;
using Esfsg.Application.Interfaces;
using Hangfire;

namespace Esfsg.API.Hangfire.Jobs
{
    public class GerarPagamentoJob : IJob
    {

        #region Construtor
        private readonly IPagamentoService _pagamentoService;
        public GerarPagamentoJob(IPagamentoService pagamentoService)
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
                await _pagamentoService.BuscarInscricoesParaPagamento();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
