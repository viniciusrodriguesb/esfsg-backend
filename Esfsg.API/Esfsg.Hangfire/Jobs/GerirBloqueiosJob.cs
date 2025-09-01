using Esfsg.Application.Interfaces;
using Esfsg.Hangfire.Configurations;
using Hangfire;

namespace Esfsg.Hangfire.Jobs
{
    public class GerirBloqueiosJob : IJob
    {

        #region Construtor
        private readonly IGestaoUsuarioService _gestaoUsuarioService;
        public GerirBloqueiosJob(IGestaoUsuarioService gestaoUsuarioService)
        {
            _gestaoUsuarioService = gestaoUsuarioService;
        }
        #endregion

        [AutomaticRetry(Attempts = 1)]
        [DisableConcurrentExecution(10000)]
        public async Task Execute()
        {
            try
            {
                await _gestaoUsuarioService.GerirBloqueiosUsuario();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
