namespace Esfsg.Domain.Models;

public partial class INSCRICAO
{
    public int Id { get; set; }

    public DateTime DhInscricao { get; set; }

    public string Periodo { get; set; } = null!;

    public bool Visita { get; set; }

    public int IdStatus { get; set; }

    public int? IdFuncaoEvento { get; set; }

    public int IdUsuario { get; set; }

    public int IdEvento { get; set; }

    public virtual ICollection<CHECK_IN> CheckIns { get; set; } = new List<CHECK_IN>();

    public virtual EVENTO IdEventoNavigation { get; set; } = null!;

    public virtual FUNCAO_EVENTO? IdFuncaoEventoNavigation { get; set; }

    public virtual STATUS IdStatusNavigation { get; set; } = null!;

    public virtual USUARIO IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<VISITA_PARTICIPANTE> VisitaParticipantes { get; set; } = new List<VISITA_PARTICIPANTE>();
}
