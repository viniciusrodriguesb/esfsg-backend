namespace Esfsg.Application.DTOs.Response
{
    public class DadosInscricaoEmailResponse
    {
        public string NomeCompleto { get; set; }
        public string Evento { get; set; }
        public int IdInscricao { get; set; }

        public string? EmailUsuario { get; set; }
        public List<int> IdsStatusInscricao { get; set; }
    }
}
