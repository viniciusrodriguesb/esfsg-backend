namespace Esfsg.Domain.Models;

public partial class CONDICAO_MEDICA
{
    public int Id { get; set; }

    public string Descricao { get; set; }

    public ICollection<USUARIO_CONDICAO_MEDICA> UsuarioCondicoesMedicas { get; set; }
}
