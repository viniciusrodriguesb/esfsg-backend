namespace Esfsg.Domain.Models
{
    public class USUARIO_FUNCAO_IGREJA
    {
        public int UsuarioId { get; set; }
        public int FuncaoIgrejaId { get; set; }

        public USUARIO UsuarioNavigation { get; set; }
        public FUNCAO_IGREJA FuncaoIgrejaNavigation { get; set; }
    }
}
