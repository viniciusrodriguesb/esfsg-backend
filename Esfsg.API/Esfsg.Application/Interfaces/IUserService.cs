namespace Esfsg.Application.Interfaces
{
    public interface IUserService
    {
        Task GetUser();
        Task GetAdministrator();
    }
}
