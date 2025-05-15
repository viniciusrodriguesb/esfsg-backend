using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class RegiaoMap : IEntityTypeConfiguration<REGIAO>
    {
        public void Configure(EntityTypeBuilder<REGIAO> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("regiao");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Nome).HasColumnName("nome");
        }
    }
}
