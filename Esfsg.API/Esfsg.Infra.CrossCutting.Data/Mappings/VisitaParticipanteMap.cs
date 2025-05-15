using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class VisitaParticipanteMap : IEntityTypeConfiguration<VISITA_PARTICIPANTE>
    {
        public void Configure(EntityTypeBuilder<VISITA_PARTICIPANTE> builder)
        {
            builder.HasKey(e => e.Id).HasName("visita_participante_pkey");

            builder.ToTable("visita_participante");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Carro).HasColumnName("carro");
            builder.Property(e => e.Funcao)
                .HasMaxLength(50)
                .HasColumnName("funcao");
            builder.Property(e => e.IdInscricao).HasColumnName("id_inscricao");
            builder.Property(e => e.IdVisita).HasColumnName("id_visita");
            builder.Property(e => e.Vagas).HasColumnName("vagas");

            builder.HasOne(d => d.IdInscricaoNavigation).WithMany(p => p.VisitaParticipantes)
                .HasForeignKey(d => d.IdInscricao);

            builder.HasOne(d => d.IdVisitaNavigation).WithMany(p => p.VisitaParticipantes)
                .HasForeignKey(d => d.IdVisita);
        }
    }
}
