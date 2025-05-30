using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enum;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class GestaoPagamentoService : IGestaoPagamentoService
    {

        #region Construtor
        private readonly DbContextBase _context;
        private readonly IPagamentoService _pagamentoService;
        public GestaoPagamentoService(DbContextBase context, IPagamentoService pagamentoService)
        {
            _context = context;
            _pagamentoService = pagamentoService;
        }
        #endregion

        public async Task<List<DadosGestaoPagamentoResponse>> ObterDadosPagamentoInscricao(string? Nome, int IdEvento)
        {
            var status = new List<int>
            {
                (int)StatusEnum.AGUARDANDO_PAGAMENTO,
                (int)StatusEnum.PAGAMENTO_CONFIRMADO
            };

            var pagamento = _context.PAGAMENTO.AsNoTracking()
                                              .Where(x => x.InscricaoNavigation.InscricaoStatus.Any(x => status.Contains(x.StatusId) && x.DhExclusao == null) &&
                                                          x.InscricaoNavigation.IdEvento == IdEvento)
                                              .Include(i => i.InscricaoNavigation)
                                                .ThenInclude(u => u.IdUsuarioNavigation)
                                               .Include(i => i.InscricaoNavigation)
                                                    .ThenInclude(x => x.IdEventoNavigation)
                                              .Include(i => i.InscricaoNavigation)
                                                    .ThenInclude(s => s.InscricaoStatus)
                                                        .ThenInclude(s => s.StatusNavigation)
                                              .AsQueryable();

            if (!string.IsNullOrEmpty(Nome))
                pagamento = pagamento.Where(x => x.InscricaoNavigation.IdUsuarioNavigation.NomeCompleto.Contains(Nome));

            var result = await pagamento.Select(x => new DadosGestaoPagamentoResponse()
            {
                IdInscricao = x.IdInscricao,
                Nome = x.InscricaoNavigation.IdUsuarioNavigation.NomeCompleto,
                Periodo = x.InscricaoNavigation.Periodo,
                DataExpiracao = x.DhExpiracao.ToString("dd/MM/yyyy"),
                Status = x.InscricaoNavigation.InscricaoStatus.FirstOrDefault(x => x.DhExclusao == null).StatusNavigation.Descricao,
                Telefone = x.InscricaoNavigation.IdUsuarioNavigation.Telefone,
                Valor = x.InscricaoNavigation.Periodo.Equals("Integral", StringComparison.OrdinalIgnoreCase) ? x.InscricaoNavigation.IdEventoNavigation.ValorIntegral : x.InscricaoNavigation.IdEventoNavigation.ValorParcial,
            }).ToListAsync();

            return result;
        }

        public async Task GerarNovoCodigoPix(int IdInscricao)
        {
            var dadoPagamento = await _context.PAGAMENTO.AsNoTracking()
                                                        .Where(x => x.IdInscricao == IdInscricao)
                                                        .FirstOrDefaultAsync();

            if (dadoPagamento.StatusRetornoApi == "approved")
                throw new ArgumentException("Pagamento desta inscrição já foi realizado.");

            if (dadoPagamento.StatusRetornoApi != "approved" && dadoPagamento.DhExpiracao.Date > DateTime.Now.Date)
                throw new ArgumentException("Ainda há um código pix em aberto para essa inscrição");

            await _pagamentoService.BuscarInscricaoPagamentoPorId(IdInscricao);
        }

    }
}
