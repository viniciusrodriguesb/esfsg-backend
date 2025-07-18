using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Interfaces
{
    public interface IGestaoUsuarioService
    {
        Task<PaginacaoResponse<GestaoUsuarioResponse>> ConsultarUsuarios(GestaoUsuarioRequest request, PaginacaoRequest paginacao);
        Task<ResultResponse<USUARIO>> AlterarRoleUsuario(AlteraRoleRequest role);
        Task<ResultResponse<USUARIO>> AlterarSenha(AlterarSenhaRequest request);
    }
}
