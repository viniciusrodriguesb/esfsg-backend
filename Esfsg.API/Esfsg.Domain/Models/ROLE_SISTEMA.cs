namespace Esfsg.Domain.Models;

public partial class ROLE_SISTEMA
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    #region Navigations
    public virtual ICollection<USUARIO> Usuarios { get; set; } = new List<USUARIO>(); 
    #endregion

}
