namespace Esfsg.Domain.Models;

public partial class VISITA_PARTICIPANTE
{
    public int Id { get; set; }

    public bool Carro { get; set; }

    public int Vagas { get; set; }

    public string? Funcao { get; set; }

    public int? IdVisita { get; set; }

    public int? IdInscricao { get; set; }

    #region Navigations
    public virtual INSCRICAO? IdInscricaoNavigation { get; set; }

    public virtual VISITA? IdVisitaNavigation { get; set; } 
    #endregion

}
