using Esfsg.Application.DTOs.Request.Relatorios;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Interfaces
{
    public interface IRelatorioService
    {
        Task<byte[]> GerarRelatorioInscricoes(RelatorioInscricaoRequest request);
        Task<byte[]> GerarRelatorioPorFuncao(int IdEvento);
    }
}
