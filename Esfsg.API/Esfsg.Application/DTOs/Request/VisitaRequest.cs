namespace Esfsg.Application.DTOs.Request
{
    public class VisitaRequest
    {
        public string Descricao { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public string Cor { get; set; } = string.Empty;
    }
}
