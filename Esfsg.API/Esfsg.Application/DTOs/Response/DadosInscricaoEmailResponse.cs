namespace Esfsg.Application.DTOs.Response
{
    public class DadosInscricaoEmailResponse
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string Evento { get; set; } = string.Empty;
        public int IdInscricao { get; set; }

        public string? EmailUsuario { get; set; }
        public List<int> IdsStatusInscricao { get; set; } = new();
    }
}
