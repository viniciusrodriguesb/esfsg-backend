namespace Esfsg.Application.DTOs.Response
{
    public class InscritosVisitaResponse
    {
        public string Nome { get; set; }
        public string FuncaoEvento { get; set; }
        public DadosInscritoVisita DadosVisita { get; set; }
    }

    public class DadosInscritoVisita
    {
        public string? NomeVisita { get; set; }
        public string? Funcao { get; set; }
        public int VagasCarro { get; set; }
        public bool Alocado { get; set; }
    }

}
