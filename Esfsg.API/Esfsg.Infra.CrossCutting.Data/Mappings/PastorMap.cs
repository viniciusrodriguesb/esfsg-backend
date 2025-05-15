using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class PastorMap : IEntityTypeConfiguration<PASTOR>
    {
        public void Configure(EntityTypeBuilder<PASTOR> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("pastor");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Nome).HasColumnName("nome");
        }
    }
}
