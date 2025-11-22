namespace Esfsg.Domain.Models;

public partial class CONDICAO_MEDICA
{
    public int Id { get; set; }

    public string Descricao { get; set; } = string.Empty;

    #region Navigations

    public ICollection<USUARIO_CONDICAO_MEDICA> UsuarioCondicoesMedicas { get; set; } = new List<USUARIO_CONDICAO_MEDICA>();
    public ICollection<MENOR_INSCRICAO> MenorInscricoes { get; set; } = new List<MENOR_INSCRICAO>();

    #endregion

}
