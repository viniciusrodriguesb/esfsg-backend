using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Esfsg.Domain.Models;

namespace Esfsg.Infra.Data.Mappings
{
    public class CheckInMap : IEntityTypeConfiguration<CHECK_IN>
    {
        public void Configure(EntityTypeBuilder<CHECK_IN> builder)
        {
            builder.HasKey(e => e.Id).HasName("check_in_pkey");
            builder.ToTable("check_in");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.IdInscricao).HasColumnName("id_inscricao");
            builder.Property(e => e.Presente).HasColumnName("presente");

            builder.HasOne(d => d.IdInscricaoNavigation).WithMany(p => p.CheckIns)
                   .HasForeignKey(d => d.IdInscricao)
                   .HasConstraintName("check_in_id_inscricao_fkey");
        }
    }
}
