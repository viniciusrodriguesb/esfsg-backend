using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class AlterarSenhaRequest
    {
        [Required(ErrorMessage = "É preciso informar o ID do usuário")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "É preciso informar a nova senha do usuário")]
        public string Senha { get; set; }
    }
}
