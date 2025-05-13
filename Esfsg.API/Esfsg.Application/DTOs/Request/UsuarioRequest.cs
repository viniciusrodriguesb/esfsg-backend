using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class UsuarioRequest
    {
        [Required]
        [StringLength(150)]
        public string NomeCompleto { get; set; }

        [Required]
        [StringLength(14)]
        public string Cpf { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Telefone { get; set; }

        [Required]
        public int Nascimento { get; set; }

        [StringLength(100)]
        [Required]
        public string Pcd { get; set; }

        [Required]
        public bool Dons { get; set; }

        [Required]
        public bool PossuiFilhos { get; set; }

        [Required]
        public int QntFilhos { get; set; }

        [Required]
        public int IdIgreja { get; set; }

        [Required]
        public int IdFuncaoIgreja { get; set; }

        [Required]
        public int IdClasse { get; set; }
    }
}
