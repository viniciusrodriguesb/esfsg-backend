namespace Esfsg.Domain.Models;

public partial class CONDICAO_MEDICA
{
    public int Id { get; set; }

    public string Descricao { get; set; }

    #region Navigations

    public ICollection<USUARIO_CONDICAO_MEDICA> UsuarioCondicoesMedicas { get; set; }
    public ICollection<MENOR_INSCRICAO> MenorInscricoes { get; set; } 

    #endregion

}
