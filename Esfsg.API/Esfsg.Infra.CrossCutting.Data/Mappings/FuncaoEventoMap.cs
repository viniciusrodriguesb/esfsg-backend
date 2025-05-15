using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class FuncaoEventoMap : IEntityTypeConfiguration<FUNCAO_EVENTO>
    {
        public void Configure(EntityTypeBuilder<FUNCAO_EVENTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("funcao_evento_pkey");

            builder.ToTable("funcao_evento");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Descricao).HasColumnName("descricao");
        }
    }
}
