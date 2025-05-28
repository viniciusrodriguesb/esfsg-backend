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
            if (string.IsNullOrEmpty(request.Cpf))
                throw new ArgumentException("É necessário enviar o CPF para prosseguir com a inscrição.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuario = await ValidarUsuario(request.Usuario, request.Cpf);

                await ValidarInscricao(request, usuario);

                var inscricao = await PersistirNovaInscricao(request, usuario);

                await ValidarInscricaoMenor(request.InscricaoMenor, inscricao.Id);

                await PersistirIgrejaInexistente(request.Igreja);

                await PersistirStatusInscricao(inscricao.Id);

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

        public async Task CancelarInscricao(int Id)
        {
            await VerificarExistenciaInscricao(Id);

            var inscricao = await _context.INSCRICAO
                                          .Where(x => x.Id == Id)
                                          .Include(s => s.InscricaoStatus)
                                          .FirstOrDefaultAsync();

            var statusAnterior = inscricao.InscricaoStatus.FirstOrDefault(x => x.DhExclusao == null);
            statusAnterior.DhExclusao = DateTime.Now;

            _context.Update(statusAnterior);
            await _context.SaveChangesAsync();

            var statusCancelado = new INSCRICAO_STATUS()
            {
                InscricaoId = inscricao.Id,
                StatusId = (int)StatusEnum.CANCELADA,
                DhInclusao = DateTime.Now
            };

            await _context.INSCRICAO_STATUS.AddAsync(statusCancelado);
            await _context.SaveChangesAsync();
        }

        #region Métodos Privados

        private async Task PersistirIgrejaInexistente(IgrejaInscricaoRequest? igreja)
        {
            if (igreja is null)
                return;

            if (string.IsNullOrWhiteSpace(igreja.Nome) || string.IsNullOrWhiteSpace(igreja.Pastor))
                throw new ArgumentException("O nome da igreja e pastor são obrigatório.");

            var existeIgreja = await _context.IGREJA
                                             .AsNoTracking()
                                             .AnyAsync(x => EF.Functions.Like(x.Nome, igreja.Nome));

            if (existeIgreja)
                throw new ArgumentException("Não foi possivel adicionar igreja pois ela já existe.");


            var pastor = await _context.PASTOR.FirstOrDefaultAsync(x => EF.Functions.Like(x.Nome, igreja.Pastor));

            if (pastor == null)
            {
                pastor = new PASTOR()
                {
                    Nome = igreja.Pastor
                };

                await _context.PASTOR.AddAsync(pastor);
            }

            var novaIgreja = new IGREJA()
            {
                Nome = igreja.Nome,
                RegiaoId = igreja.IdRegiao,
                PastorId = pastor.Id
            };

            await _context.IGREJA.AddAsync(novaIgreja);
            await _context.SaveChangesAsync();
        }

        private async Task ValidarInscricaoMenor(List<MenorRequest>? menores, int IdInscricao)
        {
            if (menores is null || menores.Count <= 0)
                return;

            var idadeInvalida = menores.Where(x => x.Idade > 7).Any();

            if (idadeInvalida)
                throw new ArgumentException("Um ou mais menores adicionados contém idade maior que 7, com isso é necessário realizar inscrição separadamente");

            var addMenores = menores.Select(x => new MENOR_INSCRICAO()
            {
                Idade = x.Idade,
                IdCondicaoMedica = x.IdCondicaoMedica,
                IdInscricao = IdInscricao
            }).ToList();

            await _context.MENOR_INSCRICAO.AddRangeAsync(addMenores);
            await _context.SaveChangesAsync();
        }

        private async Task VerificarExistenciaInscricao(int Id)
        {
            var existe = await _context.INSCRICAO.AsNoTracking()
                                                 .AnyAsync(x => x.Id == Id);

            if (!existe)
                throw new KeyNotFoundException("Não foi possivel encontrar a inscrição.");
        }

        private async Task PersistirStatusInscricao(int IdInscricao)
        {
            var enviada = new INSCRICAO_STATUS()
            {
                InscricaoId = IdInscricao,
                StatusId = (int)StatusEnum.ENVIADA,
                DhInclusao = DateTime.Now,
                DhExclusao = null
            };

            await _context.INSCRICAO_STATUS.AddAsync(enviada);
            await _context.SaveChangesAsync();

            var liberacao = new INSCRICAO_STATUS()
            {
                InscricaoId = IdInscricao,
                StatusId = (int)StatusEnum.AGUARDANDO_LIBERACAO,
                DhInclusao = DateTime.Now,
                DhExclusao = null
            };

            await _context.INSCRICAO_STATUS.AddAsync(liberacao);
            await _context.SaveChangesAsync();
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

        private async Task<USUARIO> ValidarUsuario(UsuarioRequest? request, string CPF)
        {
            var usuario = await _usuarioService.ConsultarUsuario(CPF);

            if (usuario == null && request != null)
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
                throw new ArgumentException("Usuário já cadastrado no evento selecionado");
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
