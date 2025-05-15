namespace Esfsg.Domain.Models;

public partial class USUARIO
{
    public int Id { get; set; }

    public string NomeCompleto { get; set; }

    public string Cpf { get; set; }

    public string Email { get; set; }

    public string? Senha { get; set; }

    public string? Telefone { get; set; }

    public DateTime Nascimento { get; set; }

    public string Pcd { get; set; } = null!;

    public bool Dons { get; set; }

    public bool PossuiFilhos { get; set; }

    public int QntFilhos { get; set; }

    public DateTime DhInscricao { get; set; }

    public DateTime? DhExclusao { get; set; }

    public int? IdTipoUsuario { get; set; }

    public int? IdIgreja { get; set; }

    public int? IdClasse { get; set; }

    #region Navigations
    public virtual CLASSE? IdClasseNavigation { get; set; }

    public virtual IGREJA? IdIgrejaNavigation { get; set; }

    public virtual ROLE_SISTEMA? IdTipoUsuarioNavigation { get; set; }

    public virtual ICollection<INSCRICAO> Inscricaos { get; set; }   

    public virtual ICollection<USUARIO_INSTRUMENTO> UsuarioInstrumentos { get; set; }

    public ICollection<USUARIO_CONDICAO_MEDICA> UsuarioCondicoesMedicas { get; set; }

    public virtual ICollection<USUARIO_FUNCAO_IGREJA> IdFuncaoIgrejas { get; set; }
    #endregion

}
