using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class AlteraEventoRequest
    {
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string? Nome { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O limite de inscrições integral deve ser maior que zero.")]
        public int? LimiteInscricoesIntegral { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O limite de inscrições parcial deve ser maior que zero.")]
        public int? LimiteInscricoesParcial { get; set; }

        public DateTime? DhEvento { get; set; }

        [Url(ErrorMessage = "O link do grupo do WhatsApp deve ser uma URL válida.")]
        public string? LinkGrupoWpp { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da inscrição integral deve ser maior que zero.")]
        public decimal? ValorInscricaoIntegral { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da inscrição parcial deve ser maior que zero.")]
        public decimal? ValorInscricaoParcial { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O ID da igreja vigília deve ser um número positivo.")]
        public int? IdIgrejaVigilia { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O ID da igreja evento deve ser um número positivo.")]
        public int? IdIgrejaEvento { get; set; }
    }
}
