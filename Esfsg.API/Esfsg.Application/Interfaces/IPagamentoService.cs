namespace Esfsg.Application.Interfaces
{
    public interface IPagamentoService
    {
        Task AlterarStatusInscricao();
        Task BuscarInscricoesParaPagamento();
        Task BuscarInscricaoPagamentoPorId(int IdInscricao);
    }
}
