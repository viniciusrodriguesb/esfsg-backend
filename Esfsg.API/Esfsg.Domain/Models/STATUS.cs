namespace Esfsg.Domain.Models;

public partial class STATUS
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    #region Navigations
    public ICollection<INSCRICAO_STATUS> InscricaoStatus { get; set; } = new List<INSCRICAO_STATUS>();
    public ICollection<EMAIL_BODY> EmailsBody { get; set; } = new List<EMAIL_BODY>(); 
    #endregion

}
