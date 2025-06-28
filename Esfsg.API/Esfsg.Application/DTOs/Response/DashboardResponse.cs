namespace Esfsg.Application.DTOs.Response
{
    public class DashboardResponse
    {
        public DadosInscritos Inscritos { get; set; }
        public DadosInscritosPeriodo InscritosPeriodo { get; set; }
        public DadosVisita InscritosVisita { get; set; }
        public DadosPagamento Arrecadacao { get; set; }
    }

    public class DadosInscritos
    {
        public DadosQuantitativo Confirmados { get; set; }
        public DadosQuantitativo AguardandoLiberacao { get; set; }
        public DadosQuantitativo Pendentes { get; set; }
        public DadosQuantitativo Cancelados { get; set; }
        public DadosQuantitativo ReembolsoSolicitado { get; set; }
    }

    public class DadosInscritosPeriodo
    {
        public int QuantidadeInscritosIntegral { get; set; }
        public int QuantidadeInscritosTarde { get; set; }
    }

    public class DadosVisita
    {
        public int InscritosDisponiveisVisita { get; set; }
        public int InscritosAlocados { get; set; }
        public int TotalVisitas { get; set; }
    }

    public class DadosQuantitativo
    {
        public int IdStatus { get; set; }
        public int Quantidade { get; set; }
        public decimal Percentual { get; set; }
    }

    public class DadosPagamento
    {
        public string Total { get; set; }
        public string ValorArrecadadoIntegral { get; set; }
        public string ValorArrecadadoParcial { get; set; }
    }

}
