namespace Esfsg.Domain.Models;

public partial class IGREJA
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Area { get; set; } = null!;

    public int RegiaoId { get; set; }

    public int PastorId { get; set; }


    public virtual ICollection<EVENTO> EventoIdIgrejaEventoNavigations { get; set; } = new List<EVENTO>();

    public virtual ICollection<EVENTO> EventoIdIgrejaVigiliaNavigations { get; set; } = new List<EVENTO>();

    public virtual PASTOR Pastor { get; set; } = null!;

    public virtual REGIAO Regiao { get; set; } = null!;

    public virtual ICollection<USUARIO> Usuarios { get; set; } = new List<USUARIO>();
}
