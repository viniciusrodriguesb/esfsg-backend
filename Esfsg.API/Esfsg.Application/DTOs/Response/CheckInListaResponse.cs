namespace Esfsg.Application.DTOs.Response
{
    public class CheckInListaResponse
    {
        public int IdCheckin { get; set; }
        public bool Presenca { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DadosIgrejaResponse Igreja { get; set; } = new DadosIgrejaResponse();
        public DadosEventoResponse Evento { get; set; } = new DadosEventoResponse();
    }

    public class DadosIgrejaResponse
    {
        public string Igreja { get; set; } = string.Empty;
        public string Classe { get; set; } = string.Empty;
    }

    public class DadosEventoResponse
    {
        public string Periodo { get; set; } = string.Empty;
        public string FuncaoEvento { get; set; } = string.Empty;
    }
}
