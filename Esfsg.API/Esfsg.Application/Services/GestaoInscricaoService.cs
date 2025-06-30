using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enums;
using Esfsg.Application.Filtros;
using Esfsg.Application.Helpers;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class GestaoInscricaoService : IGestaoInscricaoService
    {

        #region Construtor
        private readonly IUsuarioService _usuarioService;
        private readonly DbContextBase _context;
        public GestaoInscricaoService(DbContextBase context, IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
            _context = context;
        }
        #endregion

        public async Task<PaginacaoResponse<InscricaoParaLiberacaoResponse>> ConsultarInscricoesParaLiberacao(string Cpf, PaginacaoRequest paginacao)
        {

            var usuario = await _usuarioService.ConsultarUsuario(Cpf);

            if (usuario.IdTipoUsuario > (int)TipoUsuarioEnum.PASTOR)
                return null;

            var query = _context.INSCRICAO.AsNoTracking()
                                          .Where(x => x.InscricaoStatus.Any(s => s.StatusId == (int)StatusEnum.AGUARDANDO_LIBERACAO
                                                                              && s.DhExclusao == null))
                                          .AsQueryable();

            if (usuario.IdTipoUsuario == (int)TipoUsuarioEnum.PASTOR)
                query = query.Where(x => x.IdUsuarioNavigation.IdIgreja == usuario.IdIgreja);

            var result = query.Select(x => new InscricaoParaLiberacaoResponse()
            {
                Nome = x.IdUsuarioNavigation.NomeCompleto,
                Classe = x.IdUsuarioNavigation.IdClasseNavigation.Descricao,
                FuncaoEvento = x.IdFuncaoEventoNavigation.Descricao,
                Idade = ValidarIdadeParticipante(x.IdUsuarioNavigation.Nascimento),
                Periodo = x.Periodo,
                UsuarioBloqueado = new DadosBloqueio()
                {
                    UsuarioBloqueado = x.IdUsuarioNavigation.DhExclusao != null,
                    MotivoBloqueio = x.IdUsuarioNavigation.DhExclusao != null ? x.IdUsuarioNavigation.MotivoExclusao : string.Empty
                },
                Dependentes = x.MenorInscricoes.Select(d => new DependenteResponse()
                {
                    NomeDependente = d.Nome,
                    IdadeDependente = d.Idade
                }).ToList()
            }).OrderBy(x => x.Nome);

            var resultadoPaginado = await result.PaginarDados(paginacao);

            return resultadoPaginado;
        }

        public async Task<PaginacaoResponse<GestaoInscricaoResponse>> ConsultarInscricoes(FiltroGestaoInscricaoRequest filtro, PaginacaoRequest paginacao)
        {
            var query = _context.INSCRICAO.AsNoTracking()
                                          .AplicarFiltro(filtro)
                                          .Select(x => new GestaoInscricaoResponse()
                                          {
                                              Nome = x.IdUsuarioNavigation.NomeCompleto,
                                              Classe = x.IdUsuarioNavigation.IdClasseNavigation.Descricao,
                                              Igreja = x.IdUsuarioNavigation.IdIgrejaNavigation.Nome,
                                              Telefone = x.IdUsuarioNavigation.Telefone,
                                              Periodo = x.Periodo,
                                              FuncaoEvento = x.IdFuncaoEventoNavigation.Descricao,
                                              QntdDependentes = x.MenorInscricoes.Count(),
                                              FuncaoVisita = x.Visita ? x.VisitaParticipantes.Select(f => f.Funcao).FirstOrDefault() : "Não optante",
                                              Status = new TabelaDominioResponse()
                                              {
                                                  Id = x.InscricaoStatus.Where(x => x.DhExclusao == null).Select(s => s.StatusId).FirstOrDefault(),
                                                  Descricao = x.InscricaoStatus.Where(x => x.DhExclusao == null).Select(s => s.StatusNavigation.Descricao).FirstOrDefault()
                                              }
                                          });

            var resultadoPaginado = await query.PaginarDados(paginacao);

            return resultadoPaginado;
        }


        private static int ValidarIdadeParticipante(DateTime nascimento)
        {
            var hoje = DateTime.Today;
            int idade = hoje.Year - nascimento.Year;

            if (hoje.Month < nascimento.Month ||
                (hoje.Month == nascimento.Month && hoje.Day < nascimento.Day))
            {
                idade--;
            }

            return idade;
        }


    }
}
