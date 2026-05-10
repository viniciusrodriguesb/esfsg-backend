namespace Esfsg.Application.DTOs.Response
{
    public class GestaoInscricaoResponse
    {
        public string Nome { get; set; } = string.Empty;
        public string Igreja { get; set; } = string.Empty;
        public string Classe { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string FuncaoEvento { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public string FuncaoVisita { get; set; } = string.Empty;
        public int QntdDependentes { get; set; }
        public TabelaDominioResponse Status { get; set; } = new TabelaDominioResponse();
    }     
}
