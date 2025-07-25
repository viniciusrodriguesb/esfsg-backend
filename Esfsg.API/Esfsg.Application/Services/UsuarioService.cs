﻿using Esfsg.Application.DTOs.Request;
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
                                           Igreja = new TabelaDominioResponse()
                                           {
                                               Id = x.IdIgrejaNavigation.Id,
                                               Descricao = x.IdIgrejaNavigation.Nome
                                           },
                                           CondicoesMedica = x.UsuarioCondicoesMedicas.Select(x => x.CondicaoMedicaNavigation.Descricao).ToList(),
                                           FuncoesIgreja = x.IdFuncaoIgrejas.Select(x => x.FuncaoIgrejaNavigation.Descricao).ToList(),
                                           Instrumentos = x.UsuarioInstrumentos.Select(x => x.IdInstrumentoNavigation.Descricao).ToList()
                                       }).FirstOrDefaultAsync();
        }

        public async Task<ResultResponse<UsuarioAdministrativoResponse>> ConsultarUsuarioAdministrativo(UsuarioAdministrativoRequest request)
        {
            var usuario = await ConsultarUsuario(request.CPF);

            if (usuario == null)
            {
                return new ResultResponse<UsuarioAdministrativoResponse>()
                {
                    Sucesso = false,
                    Mensagem = "Usuário não encontrado, por favor contacte a secretaria."
                };
            }

            if (usuario.IdTipoUsuario == (int)TipoUsuarioEnum.PARTICIPANTE)
            {
                return new ResultResponse<UsuarioAdministrativoResponse>()
                {
                    Sucesso = false,
                    Mensagem = "Usuário não tem permissão de acesso, por favor entre em contato com a secretaria."
                };
            }

            var passwordHasher = new PasswordHasher<USUARIO>();
            var result = passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, request.Senha);

            if (result != PasswordVerificationResult.Success)
            {
                return new ResultResponse<UsuarioAdministrativoResponse>()
                {
                    Sucesso = false,
                    Mensagem = "Senha incorreta. Tente novamente"
                };
            }

            return new ResultResponse<UsuarioAdministrativoResponse>()
            {
                Sucesso = true,
                Mensagem = "Login realizado com sucesso.",
                Dados = new UsuarioAdministrativoResponse()
                {
                    Id = usuario.Id,
                    Nome = usuario.NomeCompleto,
                    Role = usuario.IdTipoUsuario,
                    Cpf = usuario.Cpf
                }
            };
        }

        public async Task<USUARIO?> ConsultarUsuario(string CPF)
        {
            return await _context.USUARIO.AsNoTracking()
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

        

        public async Task<ResultResponse<UsuarioEditadoResponse>> AlterarUsuario(AlterarUsuarioRequest request)
        {
            var usuario = await _context.USUARIO.FindAsync(request.IdUsuario);

            if (usuario == null)
            {
                return new ResultResponse<UsuarioEditadoResponse>
                {
                    Sucesso = false,
                    Mensagem = "Usuário não encontrado."
                };
            }

            if (!string.IsNullOrWhiteSpace(request.NomeCompleto))
                usuario.NomeCompleto = request.NomeCompleto;

            if (!string.IsNullOrWhiteSpace(request.Cpf))
                usuario.Cpf = request.Cpf.Trim();

            if (!string.IsNullOrWhiteSpace(request.Email))
                usuario.Email = request.Email.Trim().ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(request.Telefone))
                usuario.Telefone = request.Telefone;

            if (request.Nascimento.HasValue)
                usuario.Nascimento = request.Nascimento.Value;

            if (!string.IsNullOrWhiteSpace(request.Pcd))
                usuario.Pcd = request.Pcd;

            if (request.Dons.HasValue)
                usuario.Dons = request.Dons.Value;

            if (request.IdIgreja.HasValue)
                usuario.IdIgreja = request.IdIgreja.Value;

            if (request.IdClasse.HasValue)
                usuario.IdClasse = request.IdClasse.Value;

            await _context.SaveChangesAsync();           

            return new ResultResponse<UsuarioEditadoResponse>
            {
                Sucesso = true,
                Mensagem = "Usuário atualizado com sucesso."
            };

        }
    }
}
