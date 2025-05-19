using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
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

                var verificaoStatus = ValidarStatusUsuario(usuario);

                var status = await PersistirStatusInscricao(inscricao.Id, verificaoStatus);

                await PersistirVisitaParticipante(inscricao.Id, request.Visita.Visita, request);

                await PersistirCheckIn(inscricao.Id);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task<InscricaoResponse?> ConsultarInscricao(InscricaoEventoResquest request)
        {
            var inscricao = await _context.INSCRICAO
                                          .AsNoTracking()
                                          .Where(x => x.IdUsuario == request.IdUsuario &&
                                                      x.IdEvento == request.IdEvento)
                                          .Include(s => s.InscricaoStatus)
                                               .ThenInclude(s => s.StatusNavigation)
                                          .Select(x => new InscricaoResponse()
                                          {
                                              Periodo = x.Periodo,
                                              DhInscricao = x.DhInscricao.ToString("dd/MM/yyyy"),
                                              Visita = x.Visita,
                                              Status = x.InscricaoStatus.Where(x => x.DhExclusao == null).FirstOrDefault().StatusNavigation.Descricao
                                          }).FirstOrDefaultAsync();

            return inscricao;
        }

        #region Métodos Privados
        private async Task<INSCRICAO_STATUS> PersistirStatusInscricao(int IdInscricao, StatusEnum statusEnum)
        {
            var status = new INSCRICAO_STATUS()
            {
                InscricaoId = IdInscricao,
                StatusId = (int)statusEnum,
                DhInclusao = DateTime.Now,
                DhExclusao = null
            };

            await _context.INSCRICAO_STATUS.AddAsync(status);
            await _context.SaveChangesAsync();

            return status;
        }
        private static StatusEnum ValidarStatusUsuario(USUARIO usuario)
        {
            if (usuario.DhExclusao != null)
                return StatusEnum.AGUARDANDO_LIBERACAO;

            return StatusEnum.ENVIADA;
        }
        private async Task PersistirVisitaParticipante(int IdInscricao, bool icVisita, InscricaoRequest request)
        {
            if (icVisita && request.Visita is null)
                return;

            var visitaParticipante = new VISITA_PARTICIPANTE()
            {
                Carro = (bool)request.Visita.Carro,
                Vagas = (int)request.Visita.Vagas,
                Funcao = "Default",
                IdInscricao = IdInscricao
            };

            await _context.VISITA_PARTICIPANTE.AddAsync(visitaParticipante);
            await _context.SaveChangesAsync();
        }
        private async Task PersistirCheckIn(int IdInscricao)
        {
            var checkin = new CHECK_IN()
            {
                IdInscricao = IdInscricao,
                Presente = false
            };

            await _context.CHECK_IN.AddAsync(checkin);
            await _context.SaveChangesAsync();
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
                Visita = request.Visita.Visita,
                IdUsuario = usuario.Id,
                IdEvento = request.IdEvento,
                IdFuncaoEvento = request.IdFuncaoEvento
            };

            await _context.INSCRICAO.AddAsync(inscricao);
            await _context.SaveChangesAsync();

            return inscricao;
        }
        #endregion

    }
}
