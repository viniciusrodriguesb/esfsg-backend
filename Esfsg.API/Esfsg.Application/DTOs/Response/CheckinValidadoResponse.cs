namespace Esfsg.Application.DTOs.Response
{
    public class CheckinValidadoResponse
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public string Pulseira { get; set; } = string.Empty;
        public string? EtiquetaVisita { get; set; }
        public List<DadosDependenteResponse> Dependente { get; set; } = new();
    }

    public class DadosDependenteResponse
    {
        public string Nome { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
    }

}
