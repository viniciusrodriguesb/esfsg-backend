namespace Esfsg.Application.DTOs.Response
{
    public class EventoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int LimiteIntegral { get; set; }
        public int LimiteParcial { get; set; }
        public Uri LinkGrupoWpp { get; set; } = new Uri("https://www.example.com");
        public decimal ValorIntegral { get; set; }
        public decimal ValorParcial { get; set; }
        public string DataEvento { get; set; } = string.Empty;
        public string IgrejaVigilia { get; set; } = string.Empty;
        public string IgrejaEvento { get; set; } = string.Empty;
        public string Regiao { get; set; } = string.Empty;
    }
}
