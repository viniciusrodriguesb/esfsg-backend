namespace Esfsg.Application.Interfaces
{
    public interface IPagamentoService
    {
        Task AlterarStatusInscricao();
        Task BuscarInscricoesParaPagamento();
        Task GerarPagamentoPixPorInscricaoAsync(int IdInscricao);
        Task AtualizarPagamentoPorWebhook(string idTransacao);
    }
}
