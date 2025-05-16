using Esfsg.Application.DTOs.Request;
using Esfsg.Domain.Models;

namespace Esfsg.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<USUARIO?> ConsultarUsuario(string CPF);
        Task<USUARIO> IncluirUsuario(UsuarioRequest request);
    }
}
