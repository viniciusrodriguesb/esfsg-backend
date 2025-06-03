namespace Esfsg.Domain.Models;

public partial class VISITA
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public string EnderecoVisitado { get; set; } = null!;

    public string? Observacoes { get; set; }

    public string CorVoluntario { get; set; } = null!;

    #region Navigations
    public virtual ICollection<VISITA_PARTICIPANTE> VisitaParticipantes { get; set; } = new List<VISITA_PARTICIPANTE>(); 
    #endregion

}
