using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class IgrejaMap : IEntityTypeConfiguration<IGREJA>
    {
        public void Configure(EntityTypeBuilder<IGREJA> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("igreja");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Nome).HasColumnName("nome");
            builder.Property(e => e.PastorId).HasColumnName("pastor_id");
            builder.Property(e => e.RegiaoId).HasColumnName("regiao_id");

            builder.HasOne(d => d.PastorNavigation).WithMany(p => p.Igrejas)
                .HasForeignKey(d => d.PastorId);

            builder.HasOne(d => d.RegiaoNavigation).WithMany(p => p.Igrejas)
                .HasForeignKey(d => d.RegiaoId);
        }
    }
}
