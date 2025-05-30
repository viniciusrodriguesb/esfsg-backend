using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esfsg.Infra.Data.Mappings
{
    public class MenorInscricaoMap : IEntityTypeConfiguration<MENOR_INSCRICAO>
    {
        public void Configure(EntityTypeBuilder<MENOR_INSCRICAO> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("menor_inscricao");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.IdInscricao).HasColumnName("id_inscricao");
            builder.Property(e => e.IdCondicaoMedica).HasColumnName("id_condicao_medica");
            builder.Property(e => e.Idade).HasColumnName("idade");
            builder.Property(e => e.Nome).HasColumnName("nome");

            builder.HasOne(d => d.InscricaoNavigation).WithMany(p => p.MenorInscricoes)
                   .HasForeignKey(d => d.IdInscricao);

            builder.HasOne(d => d.CondicaoMedicaNavigation).WithMany(p => p.MenorInscricoes)
                   .HasForeignKey(d => d.IdCondicaoMedica);

        }
    }
}
