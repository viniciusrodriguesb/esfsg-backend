using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class InscricoesPendentesRequest
    {
        [Required(ErrorMessage = "CPF logado é obrigatório")]
        public string CpfLogado { get; set; } = string.Empty;

        [Required(ErrorMessage = "ID do evento é obrigatório")]
        public int IdEvento { get; set; }
    }
}
