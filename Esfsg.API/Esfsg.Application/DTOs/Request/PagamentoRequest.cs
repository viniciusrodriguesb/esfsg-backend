using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class PagamentoRequest
    {
        [Required]
        public decimal Valor { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string CPF { get; set; } = string.Empty;

        [Required]
        public int IdInscricao { get; set; }

    }
}
