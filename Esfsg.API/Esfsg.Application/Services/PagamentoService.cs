using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Esfsg.Application.Services
{
    public class PagamentoService : IPagamentoService
    {

        #region Construtor
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly DbContextBase _context;
        private readonly IStatusService _statusService;
        public PagamentoService(IHttpClientFactory httpClientFactory,
                                IConfiguration configuration,
                                IStatusService statusService,
                                DbContextBase context)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _context = context;
            _statusService = statusService;
        }
        #endregion

        public async Task BuscarInscricoesParaPagamento()
        {
            var inscricoes = await _context.INSCRICAO
                                    .AsNoTracking()
                                    .Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.AGUARDANDO_PAGAMENTO
                                                                           && s.DhExclusao == null))
                                    .Select(x => new PagamentoRequest()
                                    {
                                        IdInscricao = x.Id,
                                        Valor = x.Periodo.Equals("Integral", StringComparison.OrdinalIgnoreCase) ? x.IdEventoNavigation.ValorIntegral : x.IdEventoNavigation.ValorParcial,
                                        CPF = x.IdUsuarioNavigation.Cpf,
                                        Email = x.IdUsuarioNavigation.Email
                                    }).ToListAsync();

            foreach (var inscricao in inscricoes)
            {
                await CriarPagamentoPixAsync(inscricao);
            }
        }

        public async Task BuscarInscricaoPagamentoPorId(int IdInscricao)
        {
            var inscricao = await _context.INSCRICAO
                                    .AsNoTracking()
                                    .Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.AGUARDANDO_PAGAMENTO
                                                                           && s.DhExclusao == null))
                                    .Select(x => new PagamentoRequest()
                                    {
                                        IdInscricao = x.Id,
                                        Valor = x.Periodo.Equals("Integral", StringComparison.OrdinalIgnoreCase) ? x.IdEventoNavigation.ValorIntegral : x.IdEventoNavigation.ValorParcial,
                                        CPF = x.IdUsuarioNavigation.Cpf,
                                        Email = x.IdUsuarioNavigation.Email
                                    }).FirstOrDefaultAsync();

            if (inscricao == null)
                throw new ArgumentException("Número de inscrição não encontrado.");

            await CriarPagamentoPixAsync(inscricao);
        }

        public async Task AlterarStatusInscricao()
        {
            var inscricoes = await _context.INSCRICAO
                                   .Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.AGUARDANDO_PAGAMENTO
                                                                          && s.DhExclusao == null))
                                   .Include(p => p.Pagamentos)
                                   .Include(s => s.InscricaoStatus)
                                   .ToListAsync();

            foreach (var inscricao in inscricoes)
            {
                var pagamento = inscricao.Pagamentos.Where(x => x.IdInscricao == inscricao.Id).FirstOrDefault();

                if (pagamento == null)
                    continue;

                var statusApi = await VerificarStatusPagamentoApiAsync(pagamento.IdTransacao);

                if (pagamento.StatusRetornoApi == statusApi)
                    continue;

                await AtualizarInformacoesInscricao(statusApi, pagamento, inscricao);
            }

        }

        #region API's Mercado Pago

        private async Task<string?> VerificarStatusPagamentoApiAsync(string idTransacao)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.mercadopago.com/v1/payments/{idTransacao}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["token_pagamento"]);

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            var status = root.GetProperty("status").GetString();
            return status;
        }

        private async Task CriarPagamentoPixAsync(PagamentoRequest dadosPagamento)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var json = MontarBodyRequisicao(dadosPagamento);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mercadopago.com/v1/payments");

            PreencherContentHeaders(request, json);

            var response = await httpClient.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            var dadosPagamentoResponse = PreencherObjetoDadosPagamento(responseContent);

            await PersistirDadosPagamento(dadosPagamento.IdInscricao, dadosPagamento, dadosPagamentoResponse, responseContent);
        }

        #endregion

        #region Métodos Privados - AlterarStatusInscricao

        private async Task AtualizarInformacoesInscricao(string? statusApi, PAGAMENTO? pagamento, INSCRICAO inscricao)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                pagamento.StatusRetornoApi = statusApi;
                _context.PAGAMENTO.Update(pagamento);

                if (string.Equals(statusApi, "approved", StringComparison.OrdinalIgnoreCase))
                    await _statusService.AtualizarStatusInscricao(StatusEnum.PAGAMENTO_CONFIRMADO, inscricao.Id);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }

        #endregion

        #region Métodos Privados - CriarPagamentoPixAsync

        private PagamentoPixResponse PreencherObjetoDadosPagamento(string? responseContent)
        {
            var dadosPagamentoResponse = new PagamentoPixResponse();

            using var document = JsonDocument.Parse(responseContent);
            var root = document.RootElement;

            var status = root.GetProperty("status").GetString();
            if (status != "pending")
            {
                dadosPagamentoResponse.Status = "error";
                dadosPagamentoResponse.ErrorMessage = "Pagamento não está em status pendente.";
            }

            var pixCode = root.GetProperty("point_of_interaction").GetProperty("transaction_data").GetProperty("qr_code").GetString();
            var qrCodeBase64 = root.GetProperty("point_of_interaction").GetProperty("transaction_data").GetProperty("qr_code_base64").GetString();
            var idTransacao = root.GetProperty("id").GetInt64().ToString();
            var dataExpiracao = root.GetProperty("date_of_expiration").GetDateTime();
            var statusPagamento = root.GetProperty("status").GetString();

            dadosPagamentoResponse.Status = "success";
            dadosPagamentoResponse.QrCode = qrCodeBase64;
            dadosPagamentoResponse.PixCopiaCola = pixCode;
            dadosPagamentoResponse.IdTransacao = idTransacao;
            dadosPagamentoResponse.DataExpiracao = dataExpiracao;

            return dadosPagamentoResponse;
        }

        private static string? MontarBodyRequisicao(PagamentoRequest dadosPagamento)
        {
            var body = new
            {
                transaction_amount = dadosPagamento.Valor,
                payment_method_id = "pix",
                payer = new
                {
                    email = dadosPagamento.Email,
                    identification = new
                    {
                        type = "CPF",
                        number = dadosPagamento.CPF
                    }
                },
                description = "Pagamento Inscrição EsF",
                date_of_expiration = DateTime.UtcNow.AddHours(240).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz")
            };

            var json = JsonSerializer.Serialize(body);

            return json;
        }

        private void PreencherContentHeaders(HttpRequestMessage? request, string? json)
        {
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["token_pagamento"]);
            request.Headers.Add("X-Idempotency-Key", $"idempotency_{Guid.NewGuid():N}");
        }

        private async Task PersistirDadosPagamento(int InscricaoId, PagamentoRequest dadosPagamento, PagamentoPixResponse retornoAPI, string? responseContent)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var pagamento = new PAGAMENTO()
                {
                    IdInscricao = InscricaoId,
                    IdTransacao = retornoAPI.IdTransacao,
                    CodigoPix = retornoAPI.PixCopiaCola,
                    QrCodeBase64 = retornoAPI.QrCode,
                    StatusRetornoApi = retornoAPI.Status,
                    DhExpiracao = retornoAPI.DataExpiracao,
                    MensagemResposta = responseContent,
                    DhInclusao = DateTime.Now
                };

                await _context.PAGAMENTO.AddAsync(pagamento);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }

        #endregion

    }
}
