namespace Esfsg.Domain.Models;

public partial class PAGAMENTO
{
    public int Id { get; set; }

    public string IdTransacao { get; set; } = null!;

    public string CodigoPix { get; set; } = null!;

    public DateTime DhInclusao { get; set; }

    public DateTime DhExpiracao { get; set; }

    public int IdInscricao { get; set; }

    public virtual INSCRICAO InscricaoNavigation { get; set; } 
}
