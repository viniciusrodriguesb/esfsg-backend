namespace Esfsg.Domain.Models;

public partial class FUNCAO_EVENTO
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public string Cor { get; set; } = null!;

    public int Quantidade { get; set; }

    #region Navigations
    public virtual ICollection<INSCRICAO> Inscricaos { get; set; } = new List<INSCRICAO>(); 
    #endregion

}
