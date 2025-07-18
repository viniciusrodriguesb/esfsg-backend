namespace Esfsg.Application.DTOs.Request
{
    public class GestaoUsuarioRequest
    {
        public string? Nome { get; set; }
        public int? IdIgreja { get; set; }
        public int? IdClasse { get; set; }
        public string? Cpf {  get; set; }
        public int? TipoUsuario { get; set; }
    }
}
