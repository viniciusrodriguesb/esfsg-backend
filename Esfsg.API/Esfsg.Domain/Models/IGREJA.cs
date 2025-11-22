namespace Esfsg.Domain.Models;

public partial class IGREJA
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int RegiaoId { get; set; }

    public int PastorId { get; set; }


    #region Navigations

    public virtual ICollection<EVENTO> EventoIdIgrejaEventoNavigations { get; set; } = new List<EVENTO>();
    public virtual ICollection<EVENTO> EventoIdIgrejaVigiliaNavigations { get; set; } = new List<EVENTO>();
    public virtual ICollection<USUARIO> Usuarios { get; set; } = new List<USUARIO>();
    public virtual PASTOR PastorNavigation { get; set; } = new PASTOR();
    public virtual REGIAO RegiaoNavigation { get; set; } = new REGIAO();

    #endregion

}
