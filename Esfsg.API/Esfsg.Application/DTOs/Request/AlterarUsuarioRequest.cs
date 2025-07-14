using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class AlterarUsuarioRequest
    {

        [Required(ErrorMessage = "É necessário enviar o ID do usuário para edita-lo")]
        public int IdUsuario { get; set; }

        public string? NomeCompleto { get; set; }

        public string? Cpf { get; set; }

        public string? Email { get; set; }

        public string? Telefone { get; set; }

        public DateTime? Nascimento { get; set; }

        public string? Pcd { get; set; }

        public bool? Dons { get; set; }

        public int? IdIgreja { get; set; }

        public int? IdClasse { get; set; }
    }
}
