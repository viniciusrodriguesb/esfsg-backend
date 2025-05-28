namespace Esfsg.Domain.Models;

public partial class IGREJA
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int RegiaoId { get; set; }

    public int PastorId { get; set; }


    #region Navigations

    public virtual ICollection<EVENTO> EventoIdIgrejaEventoNavigations { get; set; }

    public virtual ICollection<EVENTO> EventoIdIgrejaVigiliaNavigations { get; set; }

    public virtual ICollection<USUARIO> Usuarios { get; set; }
    public virtual PASTOR PastorNavigation { get; set; }

    public virtual REGIAO RegiaoNavigation { get; set; }

    #endregion

}
