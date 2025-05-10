using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class PagamentoMap : IEntityTypeConfiguration<PAGAMENTO>
    {
        public void Configure(EntityTypeBuilder<PAGAMENTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("pagamento_pkey");

            builder.ToTable("pagamento");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.CodigoPix)
                .HasMaxLength(1000)
                .HasColumnName("codigo_pix");
            builder.Property(e => e.DhExpiracao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_expiracao");
            builder.Property(e => e.DhInclusao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_inclusao");
            builder.Property(e => e.IdTransacao)
                .HasMaxLength(30)
                .HasColumnName("id_transacao");
            builder.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            builder.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pagamento_id_usuario_fkey");
        }
    }
}
