using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;

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
    }
}
