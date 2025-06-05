using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<USUARIO?> ConsultarUsuario(string CPF);
        Task<USUARIO> IncluirUsuario(UsuarioRequest request);
        Task<UsuarioResponse?> ConsultarUsuarioLogin(string CPF);
        Task<ResultResponse<USUARIO>> ConsultarUsuarioAdministrativo(UsuarioAdministrativoRequest request);
        Task<ResultResponse<USUARIO>> AlterarRoleUsuario(AlteraRoleRequest role);
        Task<ResultResponse<USUARIO>> AlterarSenha(AlterarSenhaRequest request);
    }
}
