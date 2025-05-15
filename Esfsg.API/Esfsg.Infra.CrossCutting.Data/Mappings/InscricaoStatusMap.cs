using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class InscricaoStatusMap : IEntityTypeConfiguration<INSCRICAO_STATUS>
    {
        public void Configure(EntityTypeBuilder<INSCRICAO_STATUS> builder)
        {
            builder.HasKey(e => new { e.InscricaoId, e.StatusId });

            builder.ToTable("inscricao_status");

            builder.Property(e => e.InscricaoId).HasColumnName("id_inscricao");
            builder.Property(e => e.StatusId).HasColumnName("id_status");

            builder.Property(e => e.DhInclusao)
                  .HasColumnType("timestamp without time zone")
                  .HasColumnName("dh_inclusao");

            builder.Property(e => e.DhExclusao)
                  .HasColumnType("timestamp without time zone")
                  .HasColumnName("dh_exclusao");

            builder.HasOne(d => d.InscricaoNavigation).WithMany(p => p.InscricaoStatus)
                .HasForeignKey(d => d.InscricaoId);

            builder.HasOne(d => d.StatusNavigation).WithMany(p => p.InscricaoStatus)
                .HasForeignKey(d => d.StatusId);
        }
    }
}
