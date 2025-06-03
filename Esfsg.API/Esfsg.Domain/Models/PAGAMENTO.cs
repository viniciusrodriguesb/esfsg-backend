namespace Esfsg.Domain.Models;

public partial class PAGAMENTO
{
    public int Id { get; set; }
    public string IdTransacao { get; set; }
    public string CodigoPix { get; set; } 
    public string QrCodeBase64 { get; set; }
    public string StatusRetornoApi { get; set; }
    public DateTime DhInclusao { get; set; }
    public DateTime DhExpiracao { get; set; }
    public string MensagemResposta { get; set; }
    public int IdInscricao { get; set; }


    #region Navigations
    public virtual INSCRICAO InscricaoNavigation { get; set; }  
    #endregion

}
