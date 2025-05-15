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


    public virtual ICollection<CHECK_IN> CheckIns { get; set; } 
    public virtual ICollection<INSCRICAO_STATUS> InscricaoStatus { get; set; } 
    public virtual EVENTO IdEventoNavigation { get; set; } 
    public virtual FUNCAO_EVENTO? IdFuncaoEventoNavigation { get; set; }
    public virtual USUARIO IdUsuarioNavigation { get; set; } 
    public virtual ICollection<VISITA_PARTICIPANTE> VisitaParticipantes { get; set; }
    public virtual ICollection<PAGAMENTO> Pagamentos { get; set; }
}
