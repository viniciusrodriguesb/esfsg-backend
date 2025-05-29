using Esfsg.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Infra.Data.Mappings
{
    public class PagamentoMap : IEntityTypeConfiguration<PAGAMENTO>
    {
        public void Configure(EntityTypeBuilder<PAGAMENTO> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("pagamento");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.CodigoPix).HasColumnName("codigo_pix");
            builder.Property(e => e.QrCodeBase64).HasColumnName("qr_code");
            builder.Property(e => e.StatusRetornoApi).HasColumnName("status_retorno_api");
            builder.Property(e => e.MensagemResposta).HasColumnName("mensagem_resposta");
            builder.Property(e => e.DhExpiracao)
                   .HasColumnType("timestamp without time zone")
                   .HasColumnName("dh_expiracao");

            builder.Property(e => e.DhInclusao)
                   .HasColumnType("timestamp without time zone")
                   .HasColumnName("dh_inclusao");

            builder.Property(e => e.IdTransacao)
                   .HasMaxLength(30)
                   .HasColumnName("id_transacao");

            builder.Property(e => e.IdInscricao).HasColumnName("id_inscricao");

            builder.HasOne(d => d.InscricaoNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.IdInscricao);
        }
    }
}
