namespace Esfsg.Application.DTOs.Response
{
    public class EventoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int LimiteIntegral { get; set; }
        public int LimiteParcial { get; set; }
        public Uri LinkGrupoWpp { get; set; }
        public decimal ValorIntegral { get; set; }
        public decimal ValorParcial { get; set; }
        public string DataEvento { get; set; }
        public string IgrejaVigilia { get; set; }
        public string IgrejaEvento { get; set; }
        public string Regiao { get; set; }
    }
}
