namespace Esfsg.Application.DTOs.Response
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public bool PossuiDons {  get; set; }
        public bool isPcd {  get; set; }
        public int Filhos {  get; set; }
        public string TipoUsuario { get; set; }
        public string Classe { get; set; }
        public List<string> CondicoesMedica { get; set; }
    }
}
