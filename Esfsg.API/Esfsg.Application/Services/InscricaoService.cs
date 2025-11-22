using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enums;
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
        public InscricaoService(DbContextBase context,
                                IUsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
        }
        #endregion

        public async Task<UsuarioResponse?> RealizarInscricao(InscricaoRequest request)
        {
            if (string.IsNullOrEmpty(request.Cpf))
                throw new BusinessException("É necessário enviar o CPF para prosseguir com a inscrição.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await PersistirIgrejaInexistente(request);

                var usuario = await ValidarUsuario(request.Usuario, request.Cpf);

                await ValidarInscricao(request, usuario);

                var inscricao = await PersistirNovaInscricao(request, usuario);

                await ValidarInscricaoMenor(request.InscricaoMenor, inscricao.Id);

                await PersistirStatusInscricao(inscricao.Id);

                await PersistirVisitaParticipante(inscricao.Id, request.Visita);

                await PersistirCheckIn(inscricao.Id);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await _usuarioService.ConsultarUsuarioLogin(usuario.Cpf);
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
                                          .Select(x => new InscricaoResponse()
                                          {
                                              Id = x.Id,
                                              Periodo = x.Periodo,
                                              DhInscricao = x.DhInscricao.ToString("dd/MM/yyyy"),
                                              Visita = x.Visita,
                                              IdStatus = x.InscricaoStatus.Where(x => x.DhExclusao == null).Select(s => s.StatusId).FirstOrDefault(),
                                              Status = x.InscricaoStatus.Where(x => x.DhExclusao == null).Select(s => s.StatusNavigation.Descricao).FirstOrDefault()
                                          }).FirstOrDefaultAsync();

            return inscricao;
        }

        #region Métodos Privados - RealizarInscricao       

        private async Task PersistirIgrejaInexistente(InscricaoRequest request)
        {
            if (request.Igreja is null)
                return;

            if (string.IsNullOrWhiteSpace(request.Igreja.Nome) || string.IsNullOrWhiteSpace(request.Igreja.Pastor))
                throw new BusinessException("O nome da igreja e pastor são obrigatório.");

            var existeIgreja = await _context.IGREJA
                                             .AsNoTracking()
                                             .AnyAsync(x => EF.Functions.Like(x.Nome, request.Igreja.Nome));

            if (existeIgreja)
                throw new BusinessException("Não foi possivel adicionar igreja pois ela já existe.");

            var pastor = await _context.PASTOR.FirstOrDefaultAsync(x => EF.Functions.Like(x.Nome, request.Igreja.Pastor));

            if (pastor == null)
            {
                pastor = new PASTOR()
                {
                    Nome = request.Igreja.Pastor
                };

                await _context.PASTOR.AddAsync(pastor);
                await _context.SaveChangesAsync();
            }

            var novaIgreja = new IGREJA()
            {
                Nome = request.Igreja.Nome,
                RegiaoId = request.Igreja.IdRegiao,
                PastorId = pastor.Id
            };

            await _context.IGREJA.AddAsync(novaIgreja);
            await _context.SaveChangesAsync();

            if (request.Usuario is not null)
                request.Usuario.IdIgreja = novaIgreja.Id;
        }

        private async Task ValidarInscricaoMenor(List<MenorRequest>? menores, int IdInscricao)
        {
            if (menores is null || menores.Count <= 0)
                return;

            var idadeInvalida = menores.Where(x => x.Idade > 7).Any();

            if (idadeInvalida)
                throw new BusinessException("Um ou mais menores adicionados contém idade maior que 7, com isso é necessário realizar inscrição separadamente");

            var addMenores = menores.Select(x => new MENOR_INSCRICAO()
            {
                Idade = x.Idade,
                Nome = x.Nome,
                IdCondicaoMedica = x.IdCondicaoMedica,
                IdInscricao = IdInscricao
            }).ToList();

            await _context.MENOR_INSCRICAO.AddRangeAsync(addMenores);
            await _context.SaveChangesAsync();
        }

        private async Task PersistirStatusInscricao(int IdInscricao)
        {
            var enviada = new INSCRICAO_STATUS()
            {
                InscricaoId = IdInscricao,
                StatusId = (int)StatusEnum.ENVIADA,
                DhInclusao = DateTime.UtcNow,
                DhExclusao = null
            };

            await _context.INSCRICAO_STATUS.AddAsync(enviada);
            await _context.SaveChangesAsync();

            enviada.DhExclusao = DateTime.UtcNow;
            _context.INSCRICAO_STATUS.Update(enviada);
            await _context.SaveChangesAsync();

            var liberacao = new INSCRICAO_STATUS()
            {
                InscricaoId = IdInscricao,
                StatusId = (int)StatusEnum.AGUARDANDO_LIBERACAO,
                DhInclusao = DateTime.UtcNow,
                DhExclusao = null
            };

            await _context.INSCRICAO_STATUS.AddAsync(liberacao);
            await _context.SaveChangesAsync();
        }

        private async Task PersistirVisitaParticipante(int IdInscricao, VisitaInscricaoRequest request)
        {
            if (!request.Visita)
                return;

            var visitaParticipante = new VISITA_PARTICIPANTE()
            {
                Carro = request.Carro.HasValue ? (bool)request.Carro : false,
                Vagas = request.Vagas.HasValue ? (int)request.Vagas : 0,
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
                throw new BusinessException("Usuário já cadastrado no evento selecionado");
        }

        private async Task<INSCRICAO> PersistirNovaInscricao(InscricaoRequest request, USUARIO usuario)
        {
            var inscricao = new INSCRICAO()
            {
                DhInscricao = DateTime.UtcNow,
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
