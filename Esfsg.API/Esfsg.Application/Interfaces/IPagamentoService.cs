using Esfsg.Application.DTOs.Request;

namespace Esfsg.Application.Interfaces
{
    public interface IPagamentoService
    {
        Task AlterarStatusInscricao();
        Task BuscarInscricoesParaPagamento();
    }
}
