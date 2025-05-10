using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class ClasseMap : IEntityTypeConfiguration<CLASSE>
    {
        public void Configure(EntityTypeBuilder<CLASSE> builder)
        {
            builder.HasKey(e => e.Id).HasName("classe_pkey");
            builder.ToTable("classe");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Descricao)
                   .HasMaxLength(50)
                   .HasColumnName("descricao");
        }
    }
}
