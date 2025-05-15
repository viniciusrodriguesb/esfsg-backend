using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esfsg.Infra.Data.Mappings
{
    public class UsuarioCondicaoMedicaMap : IEntityTypeConfiguration<USUARIO_CONDICAO_MEDICA>
    {
        public void Configure(EntityTypeBuilder<USUARIO_CONDICAO_MEDICA> builder)
        {
            builder.HasKey(e => new { e.CondicaoMedicaId, e.UsuarioId });

            builder.ToTable("usuario_condicao_medica");

            builder.Property(e => e.UsuarioId).HasColumnName("id_usuario");
            builder.Property(e => e.CondicaoMedicaId).HasColumnName("id_condicao_medica");

            builder.HasOne(d => d.UsuarioNavigation).WithMany(p => p.UsuarioCondicoesMedicas)
                .HasForeignKey(d => d.UsuarioId);

            builder.HasOne(d => d.CondicaoMedicaNavigation).WithMany(p => p.UsuarioCondicoesMedicas)
                .HasForeignKey(d => d.CondicaoMedicaId);
        }
    }
}
