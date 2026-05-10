namespace Esfsg.Application.DTOs.Response
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime Nascimento { get; set; }
        public string Pcd { get; set; } = string.Empty;
        public bool PossuiDons { get; set; }
        public bool UsuarioBloqueado { get; set; }
        public TabelaDominioResponse TipoUsuario { get; set; } = new TabelaDominioResponse();
        public TabelaDominioResponse Classe { get; set; } = new TabelaDominioResponse();
        public TabelaDominioResponse Igreja { get; set; } = new TabelaDominioResponse();
        public List<string> CondicoesMedica { get; set; } = new List<string>();
        public List<string> FuncoesIgreja { get; set; } = new List<string>();
        public List<string> Instrumentos { get; set; } = new List<string>();
    }

    public class DadosUsuario
    {
        public int? TipoUsuario { get; set; }
        public int PastorId { get; set; }
    }
}
