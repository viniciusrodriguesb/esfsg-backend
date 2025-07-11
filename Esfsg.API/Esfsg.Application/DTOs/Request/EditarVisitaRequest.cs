using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class EditarVisitaRequest
    {
        [Required(ErrorMessage = "Para editar é necessário inserir o Id da visita.")]
        public int Id { get; set; }

        public string? Descricao { get; set; }

        public string? EnderecoVisitado { get; set; } 

        public string? Observacoes { get; set; }

        public string? CorVoluntario { get; set; } 
    }
}
