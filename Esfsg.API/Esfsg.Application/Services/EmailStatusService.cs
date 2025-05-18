using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enum;
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

        //Todo: validar se o body vai ser diferente para cada status
        //Se for, terá q ter uma service para cada, se não, dá para otimizar em um unico metodo genérico

        public async Task EnviarEmailQrCode()
        {
            const string subtitulo = "ESFRSG - Pagamento confirmado";
            var dadosUsuarios = await ObterInformacaoInscricao();

            var bodyQrcode = await ObterBodyEmail(StatusEnum.PAGAMENTO_CONFIRMADO);

            foreach (var usuario in dadosUsuarios)
            {
                var envioValido = await ValidarEnvioEmailStatus(StatusEnum.PAGAMENTO_CONFIRMADO, usuario);

                if (!envioValido)
                    continue;

                var qrCodeAcessoImagem = await _qrCodeService.GerarQRCodeAcesso(usuario.IdInscricao);

                var map = new Dictionary<string, string>()
                {
                    {"{nome}", usuario.NomeCompleto },
                    {"{regiao}", usuario.Regiao },
                    {"{dthr_insc}", usuario.DataInscricao },
                    {"{periodo}", usuario.Periodo },
                    {"{igreja}", usuario.IgrejaUsuario },
                    {"{classe}", usuario.ClasseUsuario },
                    {"{grupo_esf}", usuario.FuncaoEvento },
                    {"{imagemBase64}", qrCodeAcessoImagem.ImagemBase64 },
                };

                var body = SubstituirAtributos(bodyQrcode, map);

                await _emailService.SendEmailAsync(usuario.EmailUsuario, subtitulo, body);
            }
        }

        public async Task EnviarEmailInscricaoRealizada()
        {
            const string subtitulo = "Inscricao para o ESF realizada";

            var dadosUsuarios = await ObterInformacaoInscricao();

            var bodyQrcode = await ObterBodyEmail(StatusEnum.ENVIADA);

            foreach (var usuario in dadosUsuarios)
            {
                var envioValido = await ValidarEnvioEmailStatus(StatusEnum.ENVIADA, usuario);

                if (!envioValido)
                    continue;

                var qrCodeAcessoImagem = await _qrCodeService.GerarQRCodeAcesso(usuario.IdInscricao);

                var map = new Dictionary<string, string>()
                {
                    {"{nome}", usuario.NomeCompleto },
                    {"{regiao}", usuario.Regiao },
                    {"{dthr_insc}", usuario.DataInscricao },
                    {"{periodo}", usuario.Periodo },
                    {"{igreja}", usuario.IgrejaUsuario },
                    {"{classe}", usuario.ClasseUsuario },
                    {"{grupo_esf}", usuario.FuncaoEvento },
                    {"{imagemBase64}", qrCodeAcessoImagem.ImagemBase64 },
                };

                var body = SubstituirAtributos(bodyQrcode, map);

                await _emailService.SendEmailAsync(usuario.EmailUsuario, subtitulo, body);

                await GravarLogEnvioEmail(StatusEnum.ENVIADA, usuario.IdInscricao);
            }
        }

        #region Métodos Privados
        private async Task GravarLogEnvioEmail(StatusEnum status, int IdInscricao)
        {
            var log = new EMAIL_LOG()
            {
                IdInscricao = IdInscricao,
                IdStatus = (int)status,
                Enviado = true,
                DhEnvio = DateTime.Now
            };

            await _context.EMAIL_LOG.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        private async Task<bool> ValidarEnvioEmailStatus(StatusEnum status, DadosInscricaoEmailResponse usuario)
        {
            if (!usuario.IdsStatusInscricao.Contains((int)status))
                return false;

            var emailJaEnviado = await _context.EMAIL_LOG
                .AsNoTracking()
                .AnyAsync(x => x.IdInscricao == usuario.IdInscricao && 
                               x.IdStatus == (int)status &&
                               x.Enviado);

            if (emailJaEnviado)
                return false;

            return true;
        }
        private async Task<List<DadosInscricaoEmailResponse>> ObterInformacaoInscricao()
        {
            return await _context.INSCRICAO
                                  .AsNoTracking()
                                  .Include(ev => ev.IdEventoNavigation)
                                     .ThenInclude(iv => iv.IdIgrejaEventoNavigation)
                                         .ThenInclude(r => r.RegiaoNavigation)
                                  .Include(fe => fe.IdFuncaoEventoNavigation)
                                  .Include(u => u.IdUsuarioNavigation)
                                     .ThenInclude(iu => iu.IdIgrejaNavigation)
                                  .Include(u => u.IdUsuarioNavigation)
                                     .ThenInclude(c => c.IdClasseNavigation)
                                  .Include(s => s.InscricaoStatus)
                                  .Select(x => new DadosInscricaoEmailResponse()
                                  {
                                      NomeCompleto = x.IdUsuarioNavigation.NomeCompleto,
                                      EmailUsuario = x.IdUsuarioNavigation.Email,
                                      ClasseUsuario = x.IdUsuarioNavigation.IdClasseNavigation.Descricao,
                                      IgrejaUsuario = x.IdUsuarioNavigation.IdIgrejaNavigation.Nome,
                                      DataInscricao = x.DhInscricao.ToString("dd/MM/yyyy"),
                                      FuncaoEvento = x.IdFuncaoEventoNavigation.Descricao,
                                      Periodo = x.Periodo,
                                      IdInscricao = x.Id,
                                      IdsStatusInscricao = x.InscricaoStatus.Select(x => x.StatusId).ToList(),
                                      Regiao = x.IdEventoNavigation.IdIgrejaEventoNavigation.RegiaoNavigation.Nome
                                  }).ToListAsync();
        }
        private async Task<string?> ObterBodyEmail(StatusEnum status)
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
