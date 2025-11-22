namespace Esfsg.Application.DTOs.Response
{
    public class DadosGestaoPagamentoResponse
    {
        public int IdInscricao { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public string DataExpiracao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
