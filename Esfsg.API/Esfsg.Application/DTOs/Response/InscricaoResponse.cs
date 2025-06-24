namespace Esfsg.Application.DTOs.Response
{
    public class InscricaoResponse
    {
        public int Id { get; set; }
        public string DhInscricao { get; set; }
        public string Periodo { get; set; }
        public int IdStatus { get; set; }
        public string Status { get; set; }
        public bool Visita { get; set; }
    }
}
