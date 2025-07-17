namespace Esfsg.Application.DTOs.Response
{
    public class VisitaResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string? Observacao { get; set; }
        public string Cor { get; set; }
    }
}
