using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esfsg.Infra.Data.Mappings
{
    public class EmailLogMap : IEntityTypeConfiguration<EMAIL_LOG>
    {
        public void Configure(EntityTypeBuilder<EMAIL_LOG> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("email_log");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.IdInscricao).HasColumnName("id_inscricao");
            builder.Property(e => e.IdStatus).HasColumnName("id_status");
            builder.Property(e => e.Enviado).HasColumnName("enviado");
            builder.Property(e => e.DhEnvio) .HasColumnType("timestamp without time zone").HasColumnName("dh_envio");
            builder.Property(e => e.Observacoes).HasColumnName("observacoes");
        }
    }
}
