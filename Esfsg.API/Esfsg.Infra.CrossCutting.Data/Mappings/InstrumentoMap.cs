using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class InstrumentoMap : IEntityTypeConfiguration<INSTRUMENTO>
    {
        public void Configure(EntityTypeBuilder<INSTRUMENTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("instrumento_pkey");

            builder.ToTable("instrumento");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
        }
    }
}
