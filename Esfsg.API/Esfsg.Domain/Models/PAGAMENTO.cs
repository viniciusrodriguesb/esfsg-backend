namespace Esfsg.Domain.Models;

public partial class PAGAMENTO
{
    public int Id { get; set; }
    public string IdTransacao { get; set; } = string.Empty;
    public string CodigoPix { get; set; } = string.Empty;  
    public string QrCodeBase64 { get; set; } = string.Empty;
    public string StatusRetornoApi { get; set; } = string.Empty;
    public DateTime DhInclusao { get; set; }
    public DateTime DhExpiracao { get; set; }
    public string MensagemResposta { get; set; } = string.Empty;
    public int IdInscricao { get; set; }


    #region Navigations
    public virtual INSCRICAO InscricaoNavigation { get; set; } = new INSCRICAO();  
    #endregion

}
