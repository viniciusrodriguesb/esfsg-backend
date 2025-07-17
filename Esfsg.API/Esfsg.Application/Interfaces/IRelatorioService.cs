using Esfsg.Application.DTOs.Request.Relatorios;

namespace Esfsg.Application.Interfaces
{
    public interface IRelatorioService
    {
        Task<byte[]> GerarRelatorioInscricoes(RelatorioInscricaoRequest request);
    }
}
