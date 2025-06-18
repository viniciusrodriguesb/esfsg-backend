using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class AlteraFuncaoEventoRequest
    {
        [Required(ErrorMessage = "Para atualizar a função é preciso enviar o ID.")]
        public int IdFuncao { get; set; }

        public string? Descricao { get; set; }
        public string? Cor { get; set; }
        public int? Qntd { get; set; }
    }
}
