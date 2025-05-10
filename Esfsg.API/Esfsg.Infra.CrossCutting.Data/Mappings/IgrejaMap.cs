using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class IgrejaMap : IEntityTypeConfiguration<IGREJA>
    {
        public void Configure(EntityTypeBuilder<IGREJA> builder)
        {
            builder.HasKey(e => e.Id).HasName("igreja_pkey");
            builder.ToTable("igreja");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Area)
                .HasMaxLength(50)
                .HasColumnName("area");
            builder.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            builder.Property(e => e.PastorId).HasColumnName("pastor_id");
            builder.Property(e => e.RegiaoId).HasColumnName("regiao_id");

            builder.HasOne(d => d.Pastor).WithMany(p => p.Igrejas)
                .HasForeignKey(d => d.PastorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("igreja_pastor_id_fkey");

            builder.HasOne(d => d.Regiao).WithMany(p => p.Igrejas)
                .HasForeignKey(d => d.RegiaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("igreja_regiao_id_fkey");
        }
    }
}
