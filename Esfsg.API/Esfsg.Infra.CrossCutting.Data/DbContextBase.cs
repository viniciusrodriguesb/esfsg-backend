using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data
{
    public class DbContextBase : DbContext
    {
        public DbContextBase(DbContextOptions<DbContextBase> options) : base(options) { }

        #region Modelos

        #endregion

        #region Mapeamento
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AI");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContextBase).Assembly);
        }
        #endregion

    }
}
