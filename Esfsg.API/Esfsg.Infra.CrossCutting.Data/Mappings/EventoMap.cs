using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class EventoMap : IEntityTypeConfiguration<EVENTO>
    {
        public void Configure(EntityTypeBuilder<EVENTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("evento_pkey");
            builder.ToTable("evento");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.DhEvento)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_evento");
            builder.Property(e => e.IdIgrejaEvento).HasColumnName("id_igreja_evento");
            builder.Property(e => e.IdIgrejaVigilia).HasColumnName("id_igreja_vigilia");
            builder.Property(e => e.LimiteIntegral).HasColumnName("limite_integral");
            builder.Property(e => e.LimiteParcial).HasColumnName("limite_parcial");
            builder.Property(e => e.LinkWpp)
                .HasMaxLength(255)
                .HasColumnName("link_wpp");
            builder.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            builder.Property(e => e.ValorIntegral).HasColumnName("valor_integral");
            builder.Property(e => e.ValorParcial).HasColumnName("valor_parcial");

            builder.HasOne(d => d.IdIgrejaEventoNavigation).WithMany(p => p.EventoIdIgrejaEventoNavigations)
                .HasForeignKey(d => d.IdIgrejaEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evento_id_igreja_evento_fkey");

            builder.HasOne(d => d.IdIgrejaVigiliaNavigation).WithMany(p => p.EventoIdIgrejaVigiliaNavigations)
                .HasForeignKey(d => d.IdIgrejaVigilia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evento_id_igreja_vigilia_fkey");
        }
    }

}
