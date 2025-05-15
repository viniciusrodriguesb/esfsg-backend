using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Enum;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class InscricaoService : IInscricaoService
    {

        #region Construtor
        private readonly DbContextBase _context;
        private readonly IUsuarioService _usuarioService;
        private readonly IPixService _pixService;
        public InscricaoService(DbContextBase context,
                                IUsuarioService usuarioService,
                                IPixService pixService)
        {
            _context = context;
            _usuarioService = usuarioService;
            _pixService = pixService;
        }
        #endregion

        public async Task RealizarInscricao(InscricaoRequest request)
        {
            if (string.IsNullOrEmpty(request.Usuario.Cpf))
                throw new ArgumentException("É necessário enviar o CPF para prosseguir com a inscrição.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {    
                var usuario = await ValidarUsuario(request.Usuario);

                await ValidarInscricao(request, usuario);

                var inscricao = await PersistirNovaInscricao(request, usuario);

                //Enviar para API Pagamentos
                //await _pixService.GerarPixInscricao();

                //inscricao.IdStatus = (int)StatusEnum.AGUARDANDO_PAGAMENTO;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch(Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
           
        }

       private async Task<USUARIO> ValidarUsuario(UsuarioRequest request)
        {
            var usuario = await _usuarioService.ConsultarUsuario(request.Cpf);

            if (usuario == null)
                usuario = await _usuarioService.IncluirUsuario(request);

            return usuario;
        }
        private async Task ValidarInscricao(InscricaoRequest request, USUARIO usuario)
        {
            var verificaInscricao = await _context.INSCRICAO
                                                  .AsNoTracking()
                                                  .Where(x => x.IdUsuario == usuario.Id &&
                                                              x.IdEvento == request.IdEvento)
                                                  .AnyAsync();

            if (verificaInscricao)
                return;
        }
        private async Task<INSCRICAO> PersistirNovaInscricao(InscricaoRequest request, USUARIO usuario)
        {
            var inscricao = new INSCRICAO()
            {
                DhInscricao = DateTime.Now,
                Periodo = request.Periodo,
                Visita = request.Visita,
                IdUsuario = usuario.Id,
                IdEvento = request.IdEvento,
                IdFuncaoEvento = request.IdFuncaoEvento
            };

            await _context.INSCRICAO.AddAsync(inscricao);
            await _context.SaveChangesAsync();

            return inscricao;
        }

    }
}
