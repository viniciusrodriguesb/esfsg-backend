namespace Esfsg.Application.DTOs.Request
{
    public class InscricaoRequest
    {
        public string Periodo { get; set; }
        public bool Visita { get; set; }
        public int IdFuncaoEvento { get; set; }
        public int IdEvento { get; set; }
        public UsuarioRequest? Usuario { get; set; }
    }
}
