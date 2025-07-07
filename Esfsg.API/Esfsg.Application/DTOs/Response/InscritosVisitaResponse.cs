namespace Esfsg.Application.DTOs.Response
{
    public class InscritosVisitaResponse
    {
        public int IdInscricao { get; set; }
        public string Nome { get; set; }
        public string FuncaoEvento { get; set; }
        public DadosInscritoVisita DadosVisita { get; set; }
    }

    public class DadosInscritoVisita
    {
        public string? NomeVisita { get; set; }
        public string? Endereco { get; set; }
        public string? Funcao { get; set; }
        public bool Carro { get; set; }
        public int VagasCarro { get; set; }
        public bool Alocado { get; set; }
    }

}
