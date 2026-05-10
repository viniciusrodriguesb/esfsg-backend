namespace Esfsg.Application.DTOs.Response
{
    public class DashboardResponse
    {
        public DadosInscritos Inscritos { get; set; } = new DadosInscritos();
        public DadosInscritosPeriodo InscritosPeriodo { get; set; } = new DadosInscritosPeriodo();
        public DadosVisita InscritosVisita { get; set; } = new DadosVisita();
        public DadosPagamento Arrecadacao { get; set; } = new DadosPagamento();
    }

    public class DadosInscritos
    {
        public DadosQuantitativo Confirmados { get; set; } = new DadosQuantitativo();
        public DadosQuantitativo AguardandoLiberacao { get; set; } = new DadosQuantitativo();
        public DadosQuantitativo Pendentes { get; set; } = new DadosQuantitativo();
        public DadosQuantitativo Cancelados { get; set; } = new DadosQuantitativo();
        public DadosQuantitativo ReembolsoSolicitado { get; set; } = new DadosQuantitativo();
    }

    public class DadosInscritosPeriodo
    {
        public int QuantidadeInscritosIntegral { get; set; }
        public int QuantidadeInscritosTarde { get; set; }
        public int QuantidadeLiberadaIntegral { get; set; }
        public int QuantidadeLiberadaTarde { get; set; }
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
        public string Total { get; set; } = string.Empty;
        public string ValorArrecadadoIntegral { get; set; } = string.Empty;
        public string ValorArrecadadoParcial { get; set; } = string.Empty;
    }

}
