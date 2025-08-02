using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Filtros;
using Esfsg.Application.Helpers;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class CheckInService : ICheckInService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public CheckInService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<PaginacaoResponse<CheckInListaResponse>> Consultar(ConsultaCheckInRequest request, PaginacaoRequest paginacao)
        {
            var query = _context.CHECK_IN.AsNoTracking()
                                           .AplicarFiltro(request)
                                           .Select(x => new CheckInListaResponse()
                                           {
                                               IdCheckin = x.Id,
                                               Presenca = x.Presente,
                                               Nome = x.IdInscricaoNavigation.IdUsuarioNavigation.NomeCompleto,
                                               Evento = new DadosEventoResponse()
                                               {
                                                   Periodo = x.IdInscricaoNavigation.Periodo,
                                                   FuncaoEvento = x.IdInscricaoNavigation.IdFuncaoEventoNavigation.Descricao
                                               },
                                               Igreja = new DadosIgrejaResponse()
                                               {
                                                   Igreja = x.IdInscricaoNavigation.IdUsuarioNavigation.IdIgrejaNavigation.Nome,
                                                   Classe = x.IdInscricaoNavigation.IdUsuarioNavigation.IdClasseNavigation.Descricao
                                               }                                                                            
                                           });

            var resultadoPaginado = await query.PaginarDados(paginacao);

            return resultadoPaginado;
        }

        public async Task<ResultResponse<List<CheckinValidadoResponse>>> ConfirmarPresencaPorId(ValidaPresencaIdRequest request)
        {
            var result = await _context.CHECK_IN.Where(x => request.Ids.Contains(x.Id))
                                               .ExecuteUpdateAsync(s => s.SetProperty(p => p.Presente, request.Presenca));

            if (result == 0)
            {
                string mensagem = request.Presenca ? "confirmar" : "zerar";
                return new ResultResponse<List<CheckinValidadoResponse>>()
                {
                    Mensagem = $"Erro ao {mensagem} a presença.",
                    Sucesso = false
                };
            }

            var response = await ConsultarDadosCheckin(request.Ids);

            return new ResultResponse<List<CheckinValidadoResponse>>()
            {
                Mensagem = request.Presenca ? "Presença confirmada com sucesso." : "Presença zerada com sucesso.",
                Sucesso = true,
                Dados = response
            };

        }

        #region Métodos Privados   
        private async Task<List<CheckinValidadoResponse>> ConsultarDadosCheckin(List<int> ids)
        {
            return await _context.CHECK_IN.AsNoTracking()
                                          .Where(x => ids.Contains(x.Id))
                                          .Select(x => new CheckinValidadoResponse()
                                          {
                                              NomeCompleto = x.IdInscricaoNavigation.IdUsuarioNavigation.NomeCompleto,
                                              Periodo = x.IdInscricaoNavigation.Periodo,
                                              Grupo = x.IdInscricaoNavigation.IdFuncaoEventoNavigation.Descricao,
                                              Pulseira = x.IdInscricaoNavigation.IdFuncaoEventoNavigation.Cor,
                                              EtiquetaVisita = x.IdInscricaoNavigation.VisitaParticipantes.Select(v => v.IdVisitaNavigation.CorVoluntario).FirstOrDefault(),
                                              Dependente = x.IdInscricaoNavigation.MenorInscricoes.Select(m => new DadosDependenteResponse()
                                              {
                                                  Nome = m.Nome,
                                                  Cor = "Roxa"
                                              }).ToList()
                                          }).ToListAsync();
        }
        #endregion

    }
}
