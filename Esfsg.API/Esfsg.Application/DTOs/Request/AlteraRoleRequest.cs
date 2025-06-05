using Esfsg.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class AlteraRoleRequest
    {
        [Required(ErrorMessage = "É preciso informar o novo tipo de usuário")]
        public TipoUsuarioEnum TipoUsuario { get; set; }

        [Required(ErrorMessage = "É preciso informar o ID do usuário")]
        public int IdUsuario { get; set; }
    }
}
