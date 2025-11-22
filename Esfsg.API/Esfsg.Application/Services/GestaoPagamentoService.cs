using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Filtros;
using Esfsg.Application.Helpers;
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

        public async Task<PaginacaoResponse<DadosGestaoPagamentoResponse>> ObterDadosPagamentoInscricao(ConsultaGestaoPagamentoRequest request, PaginacaoRequest paginacao)
        {
            var query = _context.INSCRICAO.AsNoTracking()
                                          .AplicarFiltroPagamento(request)
                                          .Select(x => new DadosGestaoPagamentoResponse()
                                          {
                                              IdInscricao = x.Id,
                                              Nome = x.IdUsuarioNavigation.NomeCompleto,
                                              Periodo = x.Periodo,
                                              DataExpiracao = x.Pagamentos.OrderByDescending(x => x.Id).Select(d => d.DhExpiracao).FirstOrDefault().ToString("dd/MM/yyyy"),
                                              Status = x.InscricaoStatus.FirstOrDefault(x => x.DhExclusao == null).StatusNavigation.Descricao,
                                              Telefone = x.IdUsuarioNavigation.Telefone,
                                              Valor = x.Periodo.Equals("Integral", StringComparison.OrdinalIgnoreCase) ? x.IdEventoNavigation.ValorIntegral : x.IdEventoNavigation.ValorParcial,
                                          });

            var resultadoPaginado = await query.PaginarDados(paginacao);

            return resultadoPaginado;
        }

        public async Task<ResultResponse<string>> GerarNovoCodigoPix(int IdInscricao)
        {
            var dadoPagamento = await _context.PAGAMENTO.AsNoTracking()
                                                        .Where(x => x.IdInscricao == IdInscricao)
                                                        .FirstOrDefaultAsync();

            if (dadoPagamento != null && dadoPagamento.StatusRetornoApi == "approved")
            {
                return new ResultResponse<string>() { Sucesso = false, Mensagem = "Pagamento desta inscrição já foi realizado." };
            }

            if (dadoPagamento != null && dadoPagamento.StatusRetornoApi != "approved" && dadoPagamento.DhExpiracao.Date > DateTime.UtcNow.Date)
            {
                return new ResultResponse<string>() { Sucesso = false, Mensagem = "Ainda há um código pix em aberto para essa inscrição" };
            }

            try
            {
                await _pagamentoService.BuscarInscricaoPagamentoPorId(IdInscricao);
                return new ResultResponse<string>() { Sucesso = true, Mensagem = "Código Pix gerado com sucesso." };
            }
            catch (Exception)
            {
                return new ResultResponse<string>() { Sucesso = false, Mensagem = "Erro ao gerar novo código Pix." };
            }
        }

    }
}
