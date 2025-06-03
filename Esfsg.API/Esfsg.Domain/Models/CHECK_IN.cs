namespace Esfsg.Domain.Models;

public partial class CHECK_IN
{
    public int Id { get; set; }

    public bool Presente { get; set; }

    public int? IdInscricao { get; set; }

    #region Navigations
    public virtual INSCRICAO? IdInscricaoNavigation { get; set; }
    #endregion

}
