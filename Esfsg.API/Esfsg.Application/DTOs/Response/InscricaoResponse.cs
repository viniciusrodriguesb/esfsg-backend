namespace Esfsg.Application.DTOs.Response
{
    public class InscricaoResponse
    {
        public DateTime DhInscricao { get; set; }
        public string Periodo { get; set; }
        public bool Visita {  get; set; }
        public string FuncaoEvento { get; set; }
    }
}
