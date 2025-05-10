namespace Esfsg.Domain.Models;

public partial class INSTRUMENTO
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<USUARIO_INSTRUMENTO> UsuarioInstrumentos { get; set; } = new List<USUARIO_INSTRUMENTO>();
}
