﻿namespace Esfsg.Domain.Models;

public partial class FUNCAO_IGREJA
{
    public int Id { get; set; }

    public string Descricao { get; set; }

    #region Navigations
    public virtual ICollection<USUARIO_FUNCAO_IGREJA> IdFuncaoIgrejas { get; set; } 
    #endregion

}
