namespace Esfsg.Application.DTOs.Response
{
    public class GestaoInscricaoResponse
    {
        public string Nome { get; set; }
        public string Igreja { get; set; }
        public string Classe { get; set; }
        public string Telefone { get; set; }
        public string FuncaoEvento { get; set; }
        public string Periodo { get; set; }
        public string FuncaoVisita { get; set; }
        public int QntdDependentes { get; set; }
        public TabelaDominioResponse Status { get; set; }
    }     
}
