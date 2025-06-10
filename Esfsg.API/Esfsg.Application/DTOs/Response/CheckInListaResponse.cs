namespace Esfsg.Application.DTOs.Response
{
    public class CheckInListaResponse
    {
        public int IdCheckin { get; set; }
        public bool Presenca { get; set; }
        public string Nome { get; set; }
        public DadosIgrejaResponse Igreja { get; set; }
        public DadosEventoResponse Evento { get; set; }
    }

    public class DadosIgrejaResponse
    {
        public string Igreja { get; set; }
        public string Classe { get; set; }
    }

    public class DadosEventoResponse
    {
        public string Periodo { get; set; }
        public string FuncaoEvento { get; set; }
    }
}
