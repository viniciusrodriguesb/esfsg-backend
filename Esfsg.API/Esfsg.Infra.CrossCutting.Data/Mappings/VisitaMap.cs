using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class VisitaMap : IEntityTypeConfiguration<VISITA>
    {
        public void Configure(EntityTypeBuilder<VISITA> builder)
        {
            builder.HasKey(e => e.Id).HasName("visita_pkey");
            builder.ToTable("visita");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.CorVoluntario)
                .HasMaxLength(50)
                .HasColumnName("cor_voluntario");
            builder.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
            builder.Property(e => e.EnderecoVisitado)
                .HasMaxLength(100)
                .HasColumnName("endereco_visitado");
            builder.Property(e => e.Observacoes)
                .HasMaxLength(100)
                .HasColumnName("observacoes");
        }
    }
}
