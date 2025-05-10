using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;

namespace Esfsg.Application.Services
{
    public class UserService : IUserService
    {

        private readonly DbContextBase _context;
        public UserService(DbContextBase context)
        {
            _context = context;
        }

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
