using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class UsuarioRequest
    {
        [StringLength(150)]
        public string? NomeCompleto { get; set; }

        [StringLength(14)]
        public string Cpf { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Telefone { get; set; }

        public DateTime Nascimento { get; set; }

        [StringLength(100)]
        public string? Pcd { get; set; }

        public bool Dons { get; set; }

        public bool PossuiFilhos { get; set; }

        public int QntFilhos { get; set; }

        public int IdIgreja { get; set; }

        public int IdFuncaoIgreja { get; set; }

        public int IdClasse { get; set; }
    }
}
