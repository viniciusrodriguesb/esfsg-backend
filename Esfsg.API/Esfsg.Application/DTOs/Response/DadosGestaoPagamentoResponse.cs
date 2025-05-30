namespace Esfsg.Application.DTOs.Response
{
    public class DadosGestaoPagamentoResponse
    {
        public int IdInscricao { get; set; }
        public string Status {  get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Periodo { get; set; }
        public string DataExpiracao { get; set; }
        public decimal Valor { get; set; }
    }
}
