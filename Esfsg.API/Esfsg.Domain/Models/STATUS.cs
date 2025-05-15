namespace Esfsg.Domain.Models;

public partial class STATUS
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public ICollection<INSCRICAO_STATUS> InscricaoStatus { get; set; }
}
