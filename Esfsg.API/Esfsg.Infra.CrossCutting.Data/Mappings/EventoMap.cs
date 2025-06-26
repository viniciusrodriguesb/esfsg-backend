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
            builder.Property(e => e.DhEvento).HasColumnType("timestamp without time zone").HasColumnName("dh_evento");
            builder.Property(e => e.IdIgrejaEvento).HasColumnName("id_igreja_evento");
            builder.Property(e => e.IdIgrejaVigilia).HasColumnName("id_igreja_vigilia");
            builder.Property(e => e.LimiteIntegral).HasColumnName("limite_integral");
            builder.Property(e => e.LimiteParcial).HasColumnName("limite_parcial");
            builder.Property(e => e.LinkWpp).HasColumnName("link_wpp");
            builder.Property(e => e.Nome).HasColumnName("nome");
            builder.Property(e => e.ValorIntegral).HasColumnName("valor_integral");
            builder.Property(e => e.ValorParcial).HasColumnName("valor_parcial");
            builder.Property(e => e.Ativo).HasColumnName("ativo");

            builder.HasOne(d => d.IdIgrejaEventoNavigation).WithMany(p => p.EventoIdIgrejaEventoNavigations)
                .HasForeignKey(d => d.IdIgrejaEvento);

            builder.HasOne(d => d.IdIgrejaVigiliaNavigation).WithMany(p => p.EventoIdIgrejaVigiliaNavigations)
                .HasForeignKey(d => d.IdIgrejaVigilia);
        }
    }

}
