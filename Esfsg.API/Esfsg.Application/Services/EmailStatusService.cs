using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class EmailStatusService : IEmailStatusService
    {

        #region Construtor
        private readonly DbContextBase _context;
        private readonly IEmailService _emailService;
        private readonly IQrCodeService _qrCodeService;
        public EmailStatusService(DbContextBase context, IEmailService emailService, IQrCodeService qrCodeService)
        {
            _context = context;
            _emailService = emailService;
            _qrCodeService = qrCodeService;
        }
        #endregion

        public async Task EnviarEmailInscricaoRealizada()
        {
            const string subtitulo = "ESFRSG - Inscrição realizada com sucesso!";

            var dadosUsuarios = await ObterInformacaoInscricao(StatusEnum.ENVIADA);

            var stringBody = await ObterBodyEmail(StatusEnum.ENVIADA);

            foreach (var usuario in dadosUsuarios)
            {
                var emailJaEnviado = await ValidarEnvioEmailStatus(StatusEnum.ENVIADA, usuario);

                if (emailJaEnviado || string.IsNullOrWhiteSpace(usuario.EmailUsuario))
                    continue;

                var map = new Dictionary<string, string>()
                {
                    {"{nome}", usuario.NomeCompleto },
                    {"{evento}", usuario.Evento }
                };

                var body = SubstituirAtributos(stringBody, map);

                try
                {
                    await _emailService.SendEmailAsync(usuario.EmailUsuario, subtitulo, body);
                    await GravarLogEnvioEmail(StatusEnum.ENVIADA, usuario.IdInscricao, true);
                }
                catch (Exception)
                {
                    await GravarLogEnvioEmail(StatusEnum.ENVIADA, usuario.IdInscricao, false);
                }

            }
        }

        public async Task EnviarEmailQrCodePagamento()
        {
            const string subtitulo = "ESFRSG - Aguardando pagamento!";

            var dadosUsuarios = await ObterInformacaoInscricao(StatusEnum.AGUARDANDO_PAGAMENTO);

            var stringBody = await ObterBodyEmail(StatusEnum.AGUARDANDO_PAGAMENTO);

            foreach (var usuario in dadosUsuarios)
            {
                var emailJaEnviado = await ValidarEnvioEmailStatus(StatusEnum.AGUARDANDO_PAGAMENTO, usuario);

                var qrCodePagamento = await ObterQrCodePagamento(usuario.IdInscricao);

                if (emailJaEnviado || string.IsNullOrWhiteSpace(usuario.EmailUsuario) || qrCodePagamento == null)
                    continue;

                var map = new Dictionary<string, string>()
                {
                    {"{nome}", usuario.NomeCompleto },
                    {"{evento}", usuario.Evento },
                    {"{imagemBase64}", qrCodePagamento.ImagemBase64 },
                };

                var body = SubstituirAtributos(stringBody, map);

                try
                {
                    await _emailService.SendEmailAsync(usuario.EmailUsuario, subtitulo, body);
                    await GravarLogEnvioEmail(StatusEnum.AGUARDANDO_PAGAMENTO, usuario.IdInscricao, true);
                }
                catch (Exception)
                {
                    await GravarLogEnvioEmail(StatusEnum.AGUARDANDO_PAGAMENTO, usuario.IdInscricao, false);
                }

            }
        }

        public async Task EnviarEmailQrCodeAcesso()
        {
            const string subtitulo = "ESFRSG - Pagamento confirmado!";
            var dadosUsuarios = await ObterInformacaoInscricao(StatusEnum.PAGAMENTO_CONFIRMADO);

            var stringBody = await ObterBodyEmail(StatusEnum.PAGAMENTO_CONFIRMADO);

            foreach (var usuario in dadosUsuarios)
            {
                var emailJaEnviado = await ValidarEnvioEmailStatus(StatusEnum.PAGAMENTO_CONFIRMADO, usuario);

                if (emailJaEnviado || string.IsNullOrWhiteSpace(usuario.EmailUsuario))
                    continue;

                var map = new Dictionary<string, string>()
                {
                    {"{nome}", usuario.NomeCompleto },
                    {"{evento}", usuario.Evento }
                };

                var body = SubstituirAtributos(stringBody, map);

                try
                {
                    await _emailService.SendEmailAsync(usuario.EmailUsuario, subtitulo, body);
                    await GravarLogEnvioEmail(StatusEnum.PAGAMENTO_CONFIRMADO, usuario.IdInscricao, true);
                }
                catch (Exception)
                {
                    await GravarLogEnvioEmail(StatusEnum.PAGAMENTO_CONFIRMADO, usuario.IdInscricao, false);
                }

            }
        }

        public async Task EnviarEmailCancelamentoInscricao()
        {
            const string subtitulo = "ESFRSG - Inscrição cancelada";
            var dadosUsuarios = await ObterInformacaoInscricao(StatusEnum.CANCELADA);

            var stringBody = await ObterBodyEmail(StatusEnum.CANCELADA);

            foreach (var usuario in dadosUsuarios)
            {
                var emailJaEnviado = await ValidarEnvioEmailStatus(StatusEnum.CANCELADA, usuario);

                if (emailJaEnviado || string.IsNullOrWhiteSpace(usuario.EmailUsuario))
                    continue;

                var map = new Dictionary<string, string>()
                {
                    {"{nome}", usuario.NomeCompleto },
                    {"{evento}", usuario.Evento }
                };

                var body = SubstituirAtributos(stringBody, map);

                try
                {
                    await _emailService.SendEmailAsync(usuario.EmailUsuario, subtitulo, body);
                    await GravarLogEnvioEmail(StatusEnum.CANCELADA, usuario.IdInscricao, true);
                }
                catch (Exception)
                {
                    await GravarLogEnvioEmail(StatusEnum.CANCELADA, usuario.IdInscricao, false);
                }

            }
        }

        public async Task EnviarEmailReembolso()
        {
            const string subtitulo = "ESFRSG - Reembolso solicitado";
            var dadosUsuarios = await ObterInformacaoInscricao(StatusEnum.REEMBOLSO_SOLICITADO);

            var stringBody = await ObterBodyEmail(StatusEnum.REEMBOLSO_SOLICITADO);

            foreach (var usuario in dadosUsuarios)
            {
                var emailJaEnviado = await ValidarEnvioEmailStatus(StatusEnum.REEMBOLSO_SOLICITADO, usuario);

                if (emailJaEnviado || string.IsNullOrWhiteSpace(usuario.EmailUsuario))
                    continue;

                var map = new Dictionary<string, string>()
                {
                    {"{nome}", usuario.NomeCompleto },
                    {"{evento}", usuario.Evento }
                };

                var body = SubstituirAtributos(stringBody, map);

                try
                {
                    await _emailService.SendEmailAsync(usuario.EmailUsuario, subtitulo, body);
                    await GravarLogEnvioEmail(StatusEnum.REEMBOLSO_SOLICITADO, usuario.IdInscricao, true);
                }
                catch (Exception)
                {
                    await GravarLogEnvioEmail(StatusEnum.REEMBOLSO_SOLICITADO, usuario.IdInscricao, false);
                }

            }
        }

        #region Métodos Privados

        private async Task<QrCodePagamentoResponse?> ObterQrCodePagamento(int IdInscricao)
        {
            return await _context.PAGAMENTO.AsNoTracking()
                                           .Where(x => x.IdInscricao == IdInscricao &&
                                                       x.DhExpiracao >= DateTime.Now)
                                           .Select(x => new QrCodePagamentoResponse()
                                           {
                                               PixCopiaCola = x.CodigoPix,
                                               ImagemBase64 = x.QrCodeBase64,
                                               DataExpiracao = x.DhExpiracao.ToString("dd/MM/yyyy")
                                           }).FirstOrDefaultAsync();
        }
        private async Task GravarLogEnvioEmail(StatusEnum status, int IdInscricao, bool enviado)
        {
            var log = new EMAIL_LOG()
            {
                IdInscricao = IdInscricao,
                IdStatus = (int)status,
                Enviado = enviado,
                DhEnvio = DateTime.Now
            };

            await _context.EMAIL_LOG.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        private async Task<bool> ValidarEnvioEmailStatus(StatusEnum status, DadosInscricaoEmailResponse usuario)
        {
            var emailJaEnviado = await _context.EMAIL_LOG.AsNoTracking()
                                                .AnyAsync(x => x.IdInscricao == usuario.IdInscricao &&
                                                               x.IdStatus == (int)status &&
                                                               x.Enviado);

            return emailJaEnviado;
        }
        private async Task<List<DadosInscricaoEmailResponse>> ObterInformacaoInscricao(StatusEnum status)
        {
            return await _context.INSCRICAO.AsNoTracking()
                                           .Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)status))
                                           .Select(x => new DadosInscricaoEmailResponse()
                                           {
                                               IdInscricao = x.Id,
                                               NomeCompleto = x.IdUsuarioNavigation.NomeCompleto,
                                               EmailUsuario = x.IdUsuarioNavigation.Email,
                                               Evento = x.IdEventoNavigation.Nome,
                                               IdsStatusInscricao = x.InscricaoStatus.Select(x => x.StatusId).ToList()
                                           }).ToListAsync();
        }
        private async Task<string> ObterBodyEmail(StatusEnum status)
        {
            return await _context.EMAIL_BODY.AsNoTracking()
                                            .Where(x => x.IdStatus == (int)status)
                                            .Select(s => s.Body)
                                            .FirstOrDefaultAsync();
        }
        private static string SubstituirAtributos(string template, Dictionary<string, string> valores)
        {
            foreach (var item in valores)
            {
                template = template.Replace(item.Key, item.Value);
            }
            return template;
        }
        #endregion

    }
}
