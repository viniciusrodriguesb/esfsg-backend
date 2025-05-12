namespace Esfsg.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task GetUser();
        Task GetAdministrator();
    }
}
