using Esfsg.Hangfire.Configurations;
using Esfsg.Infra.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Hangfire.Jobs
{
    public class ExampleJob : IJob
    {

        private readonly DbContextBase _context;
        public ExampleJob(DbContextBase context)
        {
            _context = context;
        }

        [AutomaticRetry(Attempts = 1)]
        [DisableConcurrentExecution(10000)]
        public async Task Execute()
        {
            try
            {
                var sql = @"UPDATE usuarios SET ano_nascimento = 2000 WHERE id=1";
                await _context.Database.ExecuteSqlRawAsync(sql);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
