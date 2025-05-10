using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class UsuarioInstrumentoMap : IEntityTypeConfiguration<USUARIO_INSTRUMENTO>
    {
        public void Configure(EntityTypeBuilder<USUARIO_INSTRUMENTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("usuario_instrumento_pkey");

            builder.ToTable("usuario_instrumento");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.IdInstrumento).HasColumnName("id_instrumento");
            builder.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            builder.HasOne(d => d.IdInstrumentoNavigation).WithMany(p => p.UsuarioInstrumentos)
                .HasForeignKey(d => d.IdInstrumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_instrumento_id_instrumento_fkey");

            builder.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioInstrumentos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_instrumento_id_usuario_fkey");
        }
    }
}
