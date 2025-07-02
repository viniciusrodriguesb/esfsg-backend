namespace Esfsg.Application.DTOs.Request
{
    public class ConsultaCheckInRequest
    {
        public string? Nome { get; set; }
        public string? Periodo { get; set; }
        public int? FuncaoEvento { get; set; }
        public bool? Validado { get; set; }
    }
}
