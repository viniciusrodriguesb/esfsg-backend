using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esfsg.Infra.Data.Mappings
{    
    public class EmailBodyMap : IEntityTypeConfiguration<EMAIL_BODY>
    {
        public void Configure(EntityTypeBuilder<EMAIL_BODY> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("email_body");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Body).HasColumnName("body");
            builder.Property(e => e.IdStatus).HasColumnName("id_status");

            builder.HasOne(d => d.StatusNavigation).WithMany(p => p.EmailsBody)
                .HasForeignKey(d => d.IdStatus);
        }
    }
}
