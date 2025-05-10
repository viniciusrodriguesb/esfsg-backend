namespace Esfsg.Domain.Models;

public partial class REGIAO
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<IGREJA> Igrejas { get; set; } = new List<IGREJA>();
}
