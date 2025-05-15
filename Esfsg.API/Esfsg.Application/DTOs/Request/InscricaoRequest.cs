namespace Esfsg.Application.DTOs.Request
{
    public class InscricaoRequest
    {
        public string Periodo { get; set; }
        
        public int IdFuncaoEvento { get; set; }
        public int IdEvento { get; set; }
        public VisitaInscricaoRequest Visita { get; set; }
        public UsuarioRequest? Usuario { get; set; }
    }

    public class VisitaInscricaoRequest
    {
        public bool Visita { get; set; }
        public int? Vagas { get; set; }
        public bool? Carro { get; set; }
    }
}
