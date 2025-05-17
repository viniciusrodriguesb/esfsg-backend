using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class EmailStatusService: IEmailStatusService
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

        public async Task EnviarEmailQrCode()
        {
            const string subtitulo = "Inscricao para o ESF realizada";
            var dadosUsuarios = await ObterInformacaoInscricao();

            var bodyQrcode = await ObterBodyEmail();
              
            foreach (var usuario in dadosUsuarios)
            {
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

        #region Métodos Privados
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
                                      Regiao = x.IdEventoNavigation.IdIgrejaEventoNavigation.RegiaoNavigation.Nome
                                  }).ToListAsync();
        }

        private async Task<string?> ObterBodyEmail()
        {
            return await _context.EMAIL_BODY.AsNoTracking()
                                                    .Where(x => x.IdStatus == null)
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
