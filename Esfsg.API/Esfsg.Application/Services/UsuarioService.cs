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

        public Task GetAdministrator()
        {
            throw new NotImplementedException();
        }

        public Task GetUser()
        {
            throw new NotImplementedException();
        }

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

            return usuario;
        }
    }
}
