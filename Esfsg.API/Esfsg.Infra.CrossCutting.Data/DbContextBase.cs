using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data
{
    public class DbContextBase : DbContext
    {
        public DbContextBase(DbContextOptions<DbContextBase> options) : base(options) { }

        #region Modelos

        public virtual DbSet<CHECK_IN> CHECK_IN { get; set; }
        public virtual DbSet<CLASSE> CLASSE { get; set; }
        public virtual DbSet<CONDICAO_MEDICA> CONDICAO_MEDICA { get; set; }
        public virtual DbSet<EVENTO> EVENTO { get; set; }
        public virtual DbSet<FUNCAO_EVENTO> FUNCAO_EVENTO { get; set; }
        public virtual DbSet<FUNCAO_IGREJA> FUNCAO_IGREJA { get; set; }
        public virtual DbSet<IGREJA> IGREJA { get; set; }
        public virtual DbSet<INSCRICAO> INSCRICAO { get; set; }
        public virtual DbSet<INSTRUMENTO> INSTRUMENTO { get; set; }
        public virtual DbSet<PAGAMENTO> PAGAMENTO { get; set; }
        public virtual DbSet<PASTOR> PASTOR { get; set; }
        public virtual DbSet<REGIAO> REGIAO { get; set; }
        public virtual DbSet<ROLE_SISTEMA> ROLE_SISTEMA { get; set; }
        public virtual DbSet<STATUS> STATUS { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<USUARIO_INSTRUMENTO> USUARIO_INSTRUMENTO { get; set; }
        public virtual DbSet<VISITA> VISITA { get; set; }
        public virtual DbSet<VISITA_PARTICIPANTE> VISITA_PARTICIPANTE { get; set; }
        public virtual DbSet<INSCRICAO_STATUS> INSCRICAO_STATUS { get; set; }
        public virtual DbSet<USUARIO_CONDICAO_MEDICA> USUARIO_CONDICAO_MEDICA { get; set; }
        public virtual DbSet<USUARIO_FUNCAO_IGREJA> USUARIO_FUNCAO_IGREJA { get; set; }
        public virtual DbSet<EMAIL_BODY> EMAIL_BODY { get; set; }
        public virtual DbSet<EMAIL_LOG> EMAIL_LOG { get; set; }

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
