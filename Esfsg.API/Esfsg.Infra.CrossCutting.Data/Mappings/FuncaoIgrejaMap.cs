using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class FuncaoIgrejaMap : IEntityTypeConfiguration<FUNCAO_IGREJA>
    {
        public void Configure(EntityTypeBuilder<FUNCAO_IGREJA> builder)
        {
            builder.HasKey(e => e.Id).HasName("funcao_igreja_pkey");

            builder.ToTable("funcao_igreja");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
        }
    }
}
