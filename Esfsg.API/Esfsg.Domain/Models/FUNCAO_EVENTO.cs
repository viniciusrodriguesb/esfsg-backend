namespace Esfsg.Domain.Models;

public partial class FUNCAO_EVENTO
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<INSCRICAO> Inscricaos { get; set; } = new List<INSCRICAO>();
}
