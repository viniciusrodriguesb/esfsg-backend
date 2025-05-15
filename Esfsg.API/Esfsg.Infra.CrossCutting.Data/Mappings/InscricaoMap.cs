using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class InscricaoMap : IEntityTypeConfiguration<INSCRICAO>
    {
        public void Configure(EntityTypeBuilder<INSCRICAO> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("inscricao");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.DhInscricao)
                   .HasColumnType("timestamp without time zone")
                   .HasColumnName("dh_inscricao");
            builder.Property(e => e.IdEvento).HasColumnName("id_evento");
            builder.Property(e => e.IdFuncaoEvento).HasColumnName("id_funcao_evento");
            builder.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            builder.Property(e => e.Periodo).HasColumnName("periodo");
            builder.Property(e => e.Visita).HasColumnName("visita");

            builder.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Inscricaos)
                .HasForeignKey(d => d.IdEvento);

            builder.HasOne(d => d.IdFuncaoEventoNavigation).WithMany(p => p.Inscricaos)
                .HasForeignKey(d => d.IdFuncaoEvento);

            builder.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Inscricaos)
                .HasForeignKey(d => d.IdUsuario);
        }
    }
}
