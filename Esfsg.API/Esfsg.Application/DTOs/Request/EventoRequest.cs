using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class EventoRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O limite de inscrições integral é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O limite de inscrições integral deve ser maior que zero.")]
        public int LimiteInscricoesIntegral { get; set; }

        [Required(ErrorMessage = "O limite de inscrições parcial é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O limite de inscrições parcial deve ser maior que zero.")]
        public int LimiteInscricoesParcial { get; set; }

        [Required(ErrorMessage = "O data do evento é obrigatória.")]
        public DateTime DhEvento { get; set; }

        [Required(ErrorMessage = "O link do grupo do WhatsApp é obrigatório.")]
        [Url(ErrorMessage = "O link do grupo do WhatsApp deve ser uma URL válida.")]
        public string LinkGrupoWpp { get; set; }

        [Required(ErrorMessage = "O valor da inscrição integral é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da inscrição integral deve ser maior que zero.")]
        public decimal ValorInscricaoIntegral { get; set; }

        [Required(ErrorMessage = "O valor da inscrição parcial é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da inscrição parcial deve ser maior que zero.")]
        public decimal ValorInscricaoParcial { get; set; }

        [Required(ErrorMessage = "O ID da igreja vigília é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID da igreja vigília deve ser um número positivo.")]
        public int IdIgrejaVigilia { get; set; }

        [Required(ErrorMessage = "O ID da igreja evento é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID da igreja evento deve ser um número positivo.")]
        public int IdIgrejaEvento { get; set; }
    }
}
