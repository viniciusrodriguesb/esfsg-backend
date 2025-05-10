using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class CondicaoMedicaMap : IEntityTypeConfiguration<CONDICAO_MEDICA>
    {
        public void Configure(EntityTypeBuilder<CONDICAO_MEDICA> builder)
        {
            builder.HasKey(e => e.Id).HasName("condicao_medica_pkey");

            builder.ToTable("condicao_medica");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Descricao)
                .HasMaxLength(100)
                .HasColumnName("descricao");
        }
    }
}
