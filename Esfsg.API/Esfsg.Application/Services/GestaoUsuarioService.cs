using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Filtros;
using Esfsg.Application.Helpers;
using Esfsg.Application.Interfaces;
using Esfsg.Domain.Models;
using Esfsg.Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Esfsg.Application.Services
{
    public class GestaoUsuarioService : IGestaoUsuarioService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public GestaoUsuarioService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<PaginacaoResponse<GestaoUsuarioResponse>> ConsultarUsuarios(GestaoUsuarioRequest request, PaginacaoRequest paginacao)
        {
            var query = _context.USUARIO
                                .AsNoTracking()
                                .AplicarFiltro(request)
                                .Select(x => new GestaoUsuarioResponse()
                                {
                                    IdUsuario = x.Id,
                                    Cpf = x.Cpf,
                                    Classe = x.IdClasseNavigation.Descricao,
                                    Nome = x.NomeCompleto,
                                    Igreja = x.IdIgrejaNavigation.Nome,
                                    TipoUsuario = x.IdTipoUsuarioNavigation.Descricao
                                }).OrderBy(x => x.Nome);

            var resultadoPaginado = await query.PaginarDados(paginacao);

            return resultadoPaginado;
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
