using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enum;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class UsuarioService : IUsuarioService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public UsuarioService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<UsuarioResponse?> ConsultarUsuarioLogin(string CPF)
        {
            return await _context.USUARIO
                                       .AsNoTracking()
                                       .Where(x => x.Cpf.Trim() == CPF.Trim())
                                       .Include(u => u.IdTipoUsuarioNavigation)
                                       .Include(u => u.IdClasseNavigation)
                                       .Include(u => u.UsuarioCondicoesMedicas)
                                            .ThenInclude(c => c.CondicaoMedicaNavigation)
                                       .Include(u => u.IdFuncaoIgrejas)
                                            .ThenInclude(c => c.FuncaoIgrejaNavigation)
                                       .Include(u => u.UsuarioInstrumentos)
                                            .ThenInclude(c => c.IdInstrumentoNavigation)
                                       .Select(x => new UsuarioResponse()
                                       {
                                           Id = x.Id,
                                           NomeCompleto = x.NomeCompleto,
                                           CPF = x.Cpf,
                                           Email = x.Email,
                                           Telefone = x.Telefone,
                                           Nascimento = x.Nascimento.ToString("dd/MM/yyyy"),
                                           Pcd = x.Pcd,
                                           PossuiDons = x.Dons,
                                           QrCodePagamento = string.Empty,
                                           DhExclusao = x.DhExclusao,
                                           TipoUsuario = new TabelaDominioResponse()
                                           {
                                               Id = x.IdTipoUsuarioNavigation.Id,
                                               Descricao = x.IdTipoUsuarioNavigation.Descricao
                                           },
                                           Classe = new TabelaDominioResponse()
                                           {
                                               Id = x.IdClasseNavigation.Id,
                                               Descricao = x.IdClasseNavigation.Descricao
                                           },
                                           CondicoesMedica = x.UsuarioCondicoesMedicas.Select(x => x.CondicaoMedicaNavigation.Descricao).ToList(),
                                           FuncoesIgreja = x.IdFuncaoIgrejas.Select(x => x.FuncaoIgrejaNavigation.Descricao).ToList(),
                                           Instrumentos = x.UsuarioInstrumentos.Select(x => x.IdInstrumentoNavigation.Descricao).ToList()
                                       }).FirstOrDefaultAsync();
        }

        public async Task<USUARIO?> ConsultarUsuario(string CPF)
        {
            return await _context.USUARIO
                                        .AsNoTracking()
                                        .Where(x => x.Cpf.Trim() == CPF.Trim())
                                        .FirstOrDefaultAsync();
        }

        public async Task<USUARIO> IncluirUsuario(UsuarioRequest request)
        {
            try
            {
                var usuario = new USUARIO()
                {
                    NomeCompleto = request.NomeCompleto,
                    Cpf = request.Cpf,
                    Email = request.Email,
                    Telefone = request.Telefone,
                    Pcd = request.Pcd,
                    Dons = request.Dons,
                    DhInscricao = DateTime.Now,
                    IdTipoUsuario = (int)TipoUsuarioEnum.PARTICIPANTE,
                    IdIgreja = request.IdIgreja,
                    Nascimento = request.Nascimento,
                    IdClasse = request.IdClasse
                };

                await _context.USUARIO.AddAsync(usuario);
                await _context.SaveChangesAsync();

                if (request.CondicoesMedicas is not null)
                {
                    var condicoes = request.CondicoesMedicas.Select(item => new USUARIO_CONDICAO_MEDICA
                    {
                        UsuarioId = usuario.Id,
                        CondicaoMedicaId = item
                    });

                    await _context.USUARIO_CONDICAO_MEDICA.AddRangeAsync(condicoes);
                }

                if (request.Instrumentos is not null)
                {
                    var instrumentos = request.Instrumentos.Select(item => new USUARIO_INSTRUMENTO
                    {
                        IdUsuario = usuario.Id,
                        IdInstrumento = item
                    });

                    await _context.USUARIO_INSTRUMENTO.AddRangeAsync(instrumentos);
                }

                if (request.FuncoesIgreja is not null)
                {
                    var funcoes = request.FuncoesIgreja.Select(item => new USUARIO_FUNCAO_IGREJA
                    {
                        UsuarioId = usuario.Id,
                        FuncaoIgrejaId = item
                    });

                    await _context.USUARIO_FUNCAO_IGREJA.AddRangeAsync(funcoes);
                }

                await _context.SaveChangesAsync();

                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
