using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class RoleSistemaMap : IEntityTypeConfiguration<ROLE_SISTEMA>
    {
        public void Configure(EntityTypeBuilder<ROLE_SISTEMA> builder)
        {
            builder.HasKey(e => e.Id).HasName("role_sistema_pkey");

            builder.ToTable("role_sistema");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Descricao).HasColumnName("descricao");
        }
    }
}
