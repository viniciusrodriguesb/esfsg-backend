using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class ConsultaVisitaRequest
    {
        [Required(ErrorMessage = "ID do evento é obrigatório")]
        public int IdEvento { get; set; }

        public bool Alocado { get; set; }
    }
}
