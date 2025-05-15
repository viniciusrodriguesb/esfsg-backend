using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class UsuarioFuncaoIgrejaMap : IEntityTypeConfiguration<USUARIO_FUNCAO_IGREJA>
    {
        public void Configure(EntityTypeBuilder<USUARIO_FUNCAO_IGREJA> builder)
        {
            builder.HasKey(e => new { e.FuncaoIgrejaId, e.UsuarioId });
            builder.ToTable("usuario_funcao_igreja");

            builder.Property(e => e.UsuarioId).HasColumnName("id_usuario");
            builder.Property(e => e.FuncaoIgrejaId).HasColumnName("id_funcao_igreja");

            builder.HasOne(d => d.UsuarioNavigation).WithMany(p => p.IdFuncaoIgrejas)
                .HasForeignKey(d => d.UsuarioId);

            builder.HasOne(d => d.FuncaoIgrejaNavigation).WithMany(p => p.IdFuncaoIgrejas)
                .HasForeignKey(d => d.FuncaoIgrejaId);
        }
    }
}
