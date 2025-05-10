namespace Esfsg.Domain.Models;

public partial class PASTOR
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<IGREJA> Igrejas { get; set; } = new List<IGREJA>();
}
