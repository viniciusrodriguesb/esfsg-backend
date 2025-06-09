using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.AspNetCore.Identity;
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
                                           UsuarioBloqueado = x.DhExclusao != null,
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

        public async Task<ResultResponse<USUARIO>> ConsultarUsuarioAdministrativo(UsuarioAdministrativoRequest request)
        {
            var usuario = await ConsultarUsuario(request.CPF);

            if (usuario == null)
            {
                return new ResultResponse<USUARIO>()
                {
                    Sucesso = false,
                    Mensagem = "Usuário não encontrado, por favor contacte a secretaria."
                };
            }

            if (usuario.IdTipoUsuario == (int)TipoUsuarioEnum.PARTICIPANTE)
            {
                return new ResultResponse<USUARIO>()
                {
                    Sucesso = false,
                    Mensagem = "Usuário não tem permissão de acesso, por favor entre em contato com a secretaria."
                };
            }

            var passwordHasher = new PasswordHasher<USUARIO>();
            var result = passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, request.Senha);

            if (result != PasswordVerificationResult.Success)
            {
                return new ResultResponse<USUARIO>()
                {
                    Sucesso = false,
                    Mensagem = "Senha incorreta. Tente novamente"
                };
            }

            return new ResultResponse<USUARIO>()
            {
                Sucesso = true,
                Mensagem = "Login realizado com sucesso."
            };
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
                var senhaCriptografada = string.IsNullOrEmpty(request.Senha)
                                         ? null
                                         : new PasswordHasher<USUARIO>().HashPassword(null!, request.Senha);

                var usuario = new USUARIO()
                {
                    NomeCompleto = request.NomeCompleto,
                    Cpf = request.Cpf,
                    Email = request.Email,
                    Telefone = request.Telefone,
                    Pcd = request.Pcd,
                    Senha = senhaCriptografada,
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

        public async Task<ResultResponse<USUARIO>> AlterarRoleUsuario(AlteraRoleRequest role)
        {

            var usuario = await _context.USUARIO.FirstOrDefaultAsync(x => x.Id == role.IdUsuario);

            if (usuario == null)
            {
                return new ResultResponse<USUARIO>()
                {
                    Sucesso = false,
                    Mensagem = "Usuário não encontrado, por favor contacte a secretaria."
                };
            }

            usuario.IdTipoUsuario = (int)role.TipoUsuario;

            _context.Update(usuario);
            var res = await _context.SaveChangesAsync();

            if (res == 0)
            {
                return new ResultResponse<USUARIO>()
                {
                    Sucesso = false,
                    Mensagem = "Erro ao atualizar o tipo de usuário. Entre em contato com a TI."
                };
            }

            return new ResultResponse<USUARIO>()
            {
                Sucesso = true,
                Mensagem = "Tipo de usuario atualizado com sucesso."
            };
        }

        public async Task<ResultResponse<USUARIO>> AlterarSenha(AlterarSenhaRequest request)
        {
            var usuario = await _context.USUARIO.FirstOrDefaultAsync(x => x.Id == request.IdUsuario);

            if (usuario == null)
            {
                return new ResultResponse<USUARIO>()
                {
                    Sucesso = false,
                    Mensagem = "Usuário não encontrado, por favor contacte a secretaria."
                };
            }

            var senhaCriptografada = new PasswordHasher<USUARIO>().HashPassword(usuario, request.Senha);
            usuario.Senha = senhaCriptografada;

            _context.Update(usuario);
            var res = await _context.SaveChangesAsync();

            if (res == 0)
            {
                return new ResultResponse<USUARIO>()
                {
                    Sucesso = false,
                    Mensagem = "Erro ao atualizar a senha. Entre em contato com a TI."
                };
            }

            return new ResultResponse<USUARIO>()
            {
                Sucesso = true,
                Mensagem = "Senha atualizada com sucesso."
            };
        }
    }
}
