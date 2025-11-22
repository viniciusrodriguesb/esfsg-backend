namespace Esfsg.Domain.Models;

public partial class INSCRICAO
{
    public int Id { get; set; }

    public DateTime DhInscricao { get; set; }

    public string Periodo { get; set; } = null!;

    public bool Visita { get; set; }

    public int? IdFuncaoEvento { get; set; }

    public int IdUsuario { get; set; }

    public int IdEvento { get; set; }


    #region Navigations

    public virtual EVENTO IdEventoNavigation { get; set; } = new EVENTO();
    public virtual FUNCAO_EVENTO IdFuncaoEventoNavigation { get; set; } = new FUNCAO_EVENTO();
    public virtual USUARIO IdUsuarioNavigation { get; set; } = new USUARIO();
    public virtual ICollection<CHECK_IN> CheckIns { get; set; } = new List<CHECK_IN>();
    public virtual ICollection<INSCRICAO_STATUS> InscricaoStatus { get; set; } = new List<INSCRICAO_STATUS>();
    public virtual ICollection<VISITA_PARTICIPANTE> VisitaParticipantes { get; set; } = new List<VISITA_PARTICIPANTE>();
    public virtual ICollection<PAGAMENTO> Pagamentos { get; set; } = new List<PAGAMENTO>();
    public virtual ICollection<MENOR_INSCRICAO> MenorInscricoes { get; set; } = new List<MENOR_INSCRICAO>();   

    #endregion

}
