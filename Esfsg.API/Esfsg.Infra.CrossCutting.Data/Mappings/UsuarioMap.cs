using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<USUARIO>
    {
        public void Configure(EntityTypeBuilder<USUARIO> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("usuario");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Cpf)
                   .HasMaxLength(14)
                   .HasColumnName("cpf");

            builder.Property(e => e.DhExclusao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_exclusao");

            builder.Property(e => e.DhInscricao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dh_inscricao");

            builder.Property(e => e.Dons).HasColumnName("dons");
            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");

            builder.Property(e => e.IdClasse).HasColumnName("id_classe");
            builder.Property(e => e.IdIgreja).HasColumnName("id_igreja");
            builder.Property(e => e.IdTipoUsuario).HasColumnName("id_tipo_usuario");
            builder.Property(e => e.Nascimento).HasColumnType("date").HasColumnName("nascimento");
            builder.Property(e => e.NomeCompleto)
                .HasMaxLength(150)
                .HasColumnName("nome_completo");
            builder.Property(e => e.Pcd)
                .HasMaxLength(100)
                .HasColumnName("pcd");
            builder.Property(e => e.PossuiFilhos).HasColumnName("possui_filhos");
            builder.Property(e => e.QntFilhos).HasColumnName("qnt_filhos");
            builder.Property(e => e.Senha)
                .HasMaxLength(100)
                .HasColumnName("senha");
            builder.Property(e => e.Telefone)
                .HasMaxLength(20)
                .HasColumnName("telefone");

            builder.HasOne(d => d.IdClasseNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdClasse)
                .HasConstraintName("usuario_id_classe_fkey");

            builder.HasOne(d => d.IdIgrejaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdIgreja)
                .HasConstraintName("usuario_id_igreja_fkey");

            builder.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoUsuario)
                .HasConstraintName("usuario_id_tipo_usuario_fkey");
        }
    }
}
