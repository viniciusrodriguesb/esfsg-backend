namespace Esfsg.Domain.Models;

public partial class USUARIO
{
    public int Id { get; set; }

    public string? NomeCompleto { get; set; }

    public string? Cpf { get; set; }

    public string? Email { get; set; }

    public string? Senha { get; set; }

    public string? Telefone { get; set; }

    public int Nascimento { get; set; }

    public string Pcd { get; set; } = null!;

    public bool Dons { get; set; }

    public bool PossuiFilhos { get; set; }

    public int QntFilhos { get; set; }

    public DateTime DhInscricao { get; set; }

    public DateTime? DhExclusao { get; set; }

    public int? IdTipoUsuario { get; set; }

    public int? IdIgreja { get; set; }

    public int? IdFuncaoIgreja { get; set; }

    public int? IdClasse { get; set; }

    public virtual CLASSE? IdClasseNavigation { get; set; }

    public virtual FUNCAO_IGREJA? IdFuncaoIgrejaNavigation { get; set; }

    public virtual IGREJA? IdIgrejaNavigation { get; set; }

    public virtual ROLE_SISTEMA? IdTipoUsuarioNavigation { get; set; }

    public virtual ICollection<INSCRICAO> Inscricaos { get; set; } = new List<INSCRICAO>();

    public virtual ICollection<PAGAMENTO> Pagamentos { get; set; } = new List<PAGAMENTO>();

    public virtual ICollection<USUARIO_INSTRUMENTO> UsuarioInstrumentos { get; set; } = new List<USUARIO_INSTRUMENTO>();

    public virtual ICollection<CONDICAO_MEDICA> IdCondicaoMedicas { get; set; } = new List<CONDICAO_MEDICA>();

    public virtual ICollection<FUNCAO_IGREJA> IdFuncaoIgrejas { get; set; } = new List<FUNCAO_IGREJA>();
}
