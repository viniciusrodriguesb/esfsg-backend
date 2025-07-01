namespace Esfsg.Application.DTOs.Response
{
    public class UsuarioAdministrativoResponse
    {
        public int Id { get; set; }
        public int? Role { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
    }
}
