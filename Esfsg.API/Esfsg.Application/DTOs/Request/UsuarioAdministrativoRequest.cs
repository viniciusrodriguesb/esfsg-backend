using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class UsuarioAdministrativoRequest
    {
        [StringLength(14)]
        [Required(ErrorMessage = "Para acessar o administrativo é informar o CPF.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Para acessar o administrativo é necessário inserir a senha.")]
        public string Senha { get; set; }
    }
}
