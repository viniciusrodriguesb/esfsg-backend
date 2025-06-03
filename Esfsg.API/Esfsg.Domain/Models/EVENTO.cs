namespace Esfsg.Domain.Models;

public partial class EVENTO
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int LimiteIntegral { get; set; }

    public int LimiteParcial { get; set; }

    public string LinkWpp { get; set; } = null!;

    public decimal ValorIntegral { get; set; }

    public decimal ValorParcial { get; set; }

    public DateTime DhEvento { get; set; }

    public int IdIgrejaVigilia { get; set; }

    public int IdIgrejaEvento { get; set; }

    #region Navigations
    public virtual IGREJA IdIgrejaEventoNavigation { get; set; } = null!;

    public virtual IGREJA IdIgrejaVigiliaNavigation { get; set; } = null!;

    public virtual ICollection<INSCRICAO> Inscricaos { get; set; } = new List<INSCRICAO>(); 
    #endregion

}
