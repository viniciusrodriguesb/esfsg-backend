using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class ConsultaCheckInRequest
    {
        [Required(ErrorMessage = "ID do evento é obrigatório")]
        public int IdEvento { get; set; }
        public string? Nome { get; set; }
        public string? Periodo { get; set; }
        public int? FuncaoEvento { get; set; }
        public bool? Validado { get; set; }
    }
}
