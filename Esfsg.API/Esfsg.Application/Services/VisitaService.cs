using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enums;
using Esfsg.Application.Filtros;
using Esfsg.Application.Helpers;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class VisitaService : IVisitaService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public VisitaService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<PaginacaoResponse<InscritosVisitaResponse>> ConsultarInscritosVisita(ConsultaVisitaRequest request, PaginacaoRequest paginacao)
        {
            var query = _context.VISITA_PARTICIPANTE
                                .AsNoTracking()
                                .AplicarFiltro(request)
                                .Select(x => new InscritosVisitaResponse()
                                {
                                    IdInscricao = x.IdInscricaoNavigation.Id,
                                    Nome = x.IdInscricaoNavigation.IdUsuarioNavigation.NomeCompleto,
                                    FuncaoEvento = x.IdInscricaoNavigation.IdFuncaoEventoNavigation.Descricao,
                                    DadosVisita = new DadosInscritoVisita()
                                    {
                                        Funcao = x.Funcao,
                                        NomeVisita = x.IdVisitaNavigation != null ? x.IdVisitaNavigation.Descricao : null,
                                        Endereco = x.IdVisitaNavigation != null ? x.IdVisitaNavigation.EnderecoVisitado : null,
                                        VagasCarro = x.Vagas,
                                        Carro = x.Carro,
                                        Alocado = x.IdVisita != null
                                    }
                                });

            var resultadoPaginado = await query.PaginarDados(paginacao);

            return resultadoPaginado;
        }

        public async Task<List<VisitaResponse>> ConsultarVisitas(string? Descricao)
        {
            var query = _context.VISITA.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(Descricao))
            {
                var nomeFiltro = Descricao.Trim().ToLower();
                query = query.Where(x => x.Descricao.ToLower().Contains(nomeFiltro));
            }

            var result = await query.Select(x => new VisitaResponse()
            {
                Id = x.Id,
                Endereco = x.EnderecoVisitado,
                Nome = x.Descricao,
                Observacao = x.Observacoes,
                Cor = x.CorVoluntario
            }).ToListAsync();

            return result;
        }

        public async Task<ResultResponse<VISITA_PARTICIPANTE>> AlocarInscritosVisita(List<AlocarVisitaRequest> alocacoes)
        {
            foreach (var alocacao in alocacoes)
            {
                var jaAlocado = await _context.VISITA_PARTICIPANTE
                                              .AsNoTracking()
                                              .Include(i => i.IdInscricaoNavigation)
                                                   .ThenInclude(u => u!.IdUsuarioNavigation)
                                              .FirstOrDefaultAsync(x => x.IdInscricao == alocacao.IdInscricao &&
                                                             x.IdVisita == alocacao.IdVisita);

                if (jaAlocado != null)
                {
                    return new ResultResponse<VISITA_PARTICIPANTE>()
                    {
                        Sucesso = false,
                        Mensagem = $"O participante {jaAlocado.IdInscricaoNavigation?.IdUsuarioNavigation.NomeCompleto} já está alocado nesta visita!",
                        Dados = jaAlocado
                    };
                }

                var participante = await _context.VISITA_PARTICIPANTE.FirstOrDefaultAsync(x => x.IdInscricao == alocacao.IdInscricao);

                if (participante == null)
                    continue;

                participante.Funcao = alocacao.Funcao.ToString();
                participante.IdVisita = alocacao.IdVisita;

                _context.VISITA_PARTICIPANTE.Update(participante);
            }

            await _context.SaveChangesAsync();

            return new ResultResponse<VISITA_PARTICIPANTE>()
            {
                Sucesso = true,
                Mensagem = $"Inscritos alocados com sucesso."
            };

        }

        public List<TabelaDominioResponse> ConsultarFuncoesVisita()
        {
            return Enum.GetValues(typeof(FuncaoVisitaEnum))
                       .Cast<FuncaoVisitaEnum>()
                       .Select(e => new TabelaDominioResponse
                       {
                           Id = (int)e,
                           Descricao = e.ToString()
                       }).OrderBy(x => x.Descricao)
                         .ToList();
        }

        public async Task<ResultResponse<VISITA>> CriarVisita(VisitaRequest visita)
        {

            var existeVisita = await _context.VISITA
                                       .AsNoTracking()
                                       .Where(x => x.CorVoluntario == visita.Cor ||
                                                   x.Descricao == visita.Descricao ||
                                                   x.EnderecoVisitado == visita.Endereco)
                                       .AnyAsync();

            if (existeVisita)
            {
                return new ResultResponse<VISITA>()
                {
                    Sucesso = false,
                    Mensagem = "Já existe visita com informações enviadas."
                };
            }

            var novaVisita = new VISITA()
            {
                Descricao = visita.Descricao,
                CorVoluntario = visita.Cor,
                EnderecoVisitado = visita.Endereco,
                Observacoes = visita.Observacoes
            };

            await _context.VISITA.AddAsync(novaVisita);
            await _context.SaveChangesAsync();

            return new ResultResponse<VISITA>()
            {
                Sucesso = true,
                Mensagem = "Visita criada com sucesso.",
                Dados = novaVisita
            };
        }

        public async Task ExcluirVisita(int Id)
        {
            await _context.VISITA
                          .Where(x => x.Id == Id)
                          .ExecuteDeleteAsync();
        }

        public async Task<ResultResponse<EditarVisitaRequest>> EditarVisitaAsync(EditarVisitaRequest request)
        {
            var visita = await _context.VISITA.FindAsync(request.Id);

            if (visita == null)
            {
                return new ResultResponse<EditarVisitaRequest>
                {
                    Sucesso = false,
                    Mensagem = "Visita não encontrada."
                };
            }

            if (!string.IsNullOrWhiteSpace(request.Descricao))
                visita.Descricao = request.Descricao;

            if (!string.IsNullOrWhiteSpace(request.CorVoluntario))
                visita.CorVoluntario = request.CorVoluntario;

            if (!string.IsNullOrWhiteSpace(request.EnderecoVisitado))
                visita.EnderecoVisitado = request.EnderecoVisitado;

            visita.Observacoes = request.Observacoes;

            _context.VISITA.Update(visita);
            await _context.SaveChangesAsync();

            var visitaAtualizada = await _context.VISITA
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new EditarVisitaRequest()
                {
                    Id = x.Id,
                    Descricao = x.Descricao,
                    CorVoluntario = x.CorVoluntario,
                    EnderecoVisitado = x.EnderecoVisitado,
                    Observacoes = x.Observacoes
                }).FirstOrDefaultAsync();

            return new ResultResponse<EditarVisitaRequest>
            {
                Sucesso = true,
                Mensagem = "Visita atualizada com sucesso.",
                Dados = visitaAtualizada
            };
        }

    }
}
