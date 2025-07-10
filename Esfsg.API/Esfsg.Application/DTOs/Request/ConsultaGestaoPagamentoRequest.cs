using Esfsg.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class ConsultaGestaoPagamentoRequest
    {
        [Required(ErrorMessage = "ID do evento é obrigatório")]
        public int IdEvento { get; set; }
        public string? Nome { get; set; }
        public StatusEnum Status { get; set; }
    }
}
