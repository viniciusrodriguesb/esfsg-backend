using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class FiltroGestaoInscricaoRequest
    {
        [Required(ErrorMessage = "ID do evento é obrigatório")]
        public int IdEvento { get; set; }
        public int? Regiao {  get; set; }
        public string? Nome { get; set; }
        public int? Igreja { get; set; }
        public int? Classe { get; set; }
        public int? FuncaoEvento { get; set; }
        public string? Periodo { get; set; }
    }
}
