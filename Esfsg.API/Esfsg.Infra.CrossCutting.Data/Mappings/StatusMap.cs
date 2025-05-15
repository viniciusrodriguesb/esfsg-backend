using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class StatusMap : IEntityTypeConfiguration<STATUS>
    {
        public void Configure(EntityTypeBuilder<STATUS> builder)
        {
            builder.HasKey(e => e.Id).HasName("status_pkey");
            builder.ToTable("status");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Descricao).HasColumnName("descricao");
        }
    }
}
