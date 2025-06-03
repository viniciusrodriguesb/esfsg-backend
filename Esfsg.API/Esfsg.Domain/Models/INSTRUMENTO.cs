namespace Esfsg.Domain.Models;

public partial class INSTRUMENTO
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    #region Navigations
    public virtual ICollection<USUARIO_INSTRUMENTO> UsuarioInstrumentos { get; set; } = new List<USUARIO_INSTRUMENTO>(); 
    #endregion

}
