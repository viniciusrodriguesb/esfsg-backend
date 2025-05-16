using Esfsg.Application.DTOs.Request;
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


        public async Task<USUARIO?> ConsultarUsuario(string CPF)
        {
            return await _context.USUARIO
                                        .AsNoTracking()
                                        .Where(x => x.Cpf.Trim() == CPF.Trim() &&
                                                    x.DhExclusao == null)
                                        .FirstOrDefaultAsync();
        }

        public async Task<USUARIO> IncluirUsuario(UsuarioRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

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
                    PossuiFilhos = request.PossuiFilhos,
                    QntFilhos = request.QntFilhos,
                    DhInscricao = DateTime.Now,
                    IdTipoUsuario = (int)TipoUsuarioEnum.PARTICIPANTE,
                    IdIgreja = request.IdIgreja,
                    IdClasse = request.IdClasse
                };

                await _context.USUARIO.AddAsync(usuario);
                await _context.SaveChangesAsync();

                if (request.CondicoesMedicas.Any())
                {
                    var condicoes = request.CondicoesMedicas.Select(item => new USUARIO_CONDICAO_MEDICA
                    {
                        UsuarioId = usuario.Id,
                        CondicaoMedicaId = item
                    });

                    await _context.USUARIO_CONDICAO_MEDICA.AddRangeAsync(condicoes);
                }

                if (request.Instrumentos.Any())
                {
                    var instrumentos = request.Instrumentos.Select(item => new USUARIO_INSTRUMENTO
                    {
                        IdUsuario = usuario.Id,
                        IdInstrumento = item
                    });

                    await _context.USUARIO_INSTRUMENTO.AddRangeAsync(instrumentos);
                }

                if (request.FuncoesIgreja.Any())
                {
                    var funcoes = request.FuncoesIgreja.Select(item => new USUARIO_FUNCAO_IGREJA
                    {
                        UsuarioId = usuario.Id,
                        FuncaoIgrejaId = item
                    });

                    await _context.USUARIO_FUNCAO_IGREJA.AddRangeAsync(funcoes);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return usuario;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
