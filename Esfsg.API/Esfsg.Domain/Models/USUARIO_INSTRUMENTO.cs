namespace Esfsg.Domain.Models;

public partial class USUARIO_INSTRUMENTO
{
    public int IdUsuario { get; set; }

    public int IdInstrumento { get; set; }

    public virtual INSTRUMENTO IdInstrumentoNavigation { get; set; } = null!;

    public virtual USUARIO IdUsuarioNavigation { get; set; } = null!;
}
