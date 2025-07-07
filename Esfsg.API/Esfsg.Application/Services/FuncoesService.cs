using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class FuncoesService : IFuncoesService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public FuncoesService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<TabelaDominioResponse>> ConsultarFuncoesIgreja()
        {
            var funcoes = await _context.FUNCAO_IGREJA
                                        .AsNoTracking()
                                        .Select(x => new TabelaDominioResponse()
                                        {
                                            Id = x.Id,
                                            Descricao = x.Descricao,
                                        }).ToListAsync();

            return funcoes;
        }

        public async Task<List<TabelaDominioResponse>> ConsultarFuncoesEvento(int IdEvento)
        {
            var funcoesDisponiveis = await _context.FUNCAO_EVENTO
                                                   .AsNoTracking()
                                                   .GroupJoin(_context.INSCRICAO.Where(x => x.IdEvento == IdEvento),
                                                               fe => fe.Id,
                                                               i => i.IdFuncaoEvento,
                                                               (fe, inscricoes) => new
                                                               {
                                                                   Funcao = fe,
                                                                   Total = inscricoes.Count()
                                                               })
                                                   .Where(x => x.Total < x.Funcao.Quantidade)
                                                   .Select(x => new TabelaDominioResponse()
                                                   {
                                                       Id = x.Funcao.Id,
                                                       Descricao = x.Funcao.Descricao,
                                                   })
                                                   .OrderBy(x => x.Id)
                                                   .ToListAsync();

            return funcoesDisponiveis;
        }

        public async Task<List<FuncaoEventoResponse>> ConsultarFuncoesEventoAdministrativo()
        {
            return await _context.FUNCAO_EVENTO.AsNoTracking()
                                               .Select(x => new FuncaoEventoResponse()
                                               {
                                                   Id = x.Id,
                                                   Descricao = x.Descricao,
                                                   Cor = x.Cor,
                                                   Quantidade = x.Quantidade
                                               })
                                               .OrderBy(x => x.Id)
                                               .ToListAsync();
        }

        public async Task<ResultResponse<FuncaoEventoResponse>> EditarFuncoesEvento(AlteraFuncaoEventoRequest request)
        {
            var funcao = await _context.FUNCAO_EVENTO.Where(f => f.Id == request.IdFuncao)
                                                     .FirstOrDefaultAsync();

            if (funcao == null)
            {
                return new ResultResponse<FuncaoEventoResponse>()
                {
                    Sucesso = false,
                    Mensagem = "Função para o evento não encontrada."
                };
            }

            if (!string.IsNullOrWhiteSpace(request.Descricao))
                funcao.Descricao = request.Descricao;

            if (!string.IsNullOrWhiteSpace(request.Cor))
                funcao.Cor = request.Cor;

            if (request.Qntd.HasValue)
                funcao.Quantidade = request.Qntd.Value;

            _context.FUNCAO_EVENTO.Update(funcao);
            await _context.SaveChangesAsync();

            var response = new FuncaoEventoResponse
            {
                Id = funcao.Id,
                Descricao = funcao.Descricao,
                Cor = funcao.Cor,
                Quantidade = funcao.Quantidade
            };

            return new ResultResponse<FuncaoEventoResponse>()
            {
                Sucesso = true,
                Mensagem = "Função do evento editada com sucesso.",
                Dados = response
            };
        }


    }
}
