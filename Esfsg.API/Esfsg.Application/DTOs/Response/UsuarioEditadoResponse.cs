namespace Esfsg.Application.DTOs.Response
{
    public class UsuarioEditadoResponse
    {
        public int IdUsuario { get; set; }

        public string NomeCompleto { get; set; }

        public string Cpf { get; set; }

        public string Email { get; set; }

        public string? Telefone { get; set; }

        public DateTime Nascimento { get; set; }

        public string? Pcd { get; set; }

        public bool Dons { get; set; }

        public TabelaDominioResponse Igreja { get; set; }

        public TabelaDominioResponse Classe { get; set; }
    }
}
