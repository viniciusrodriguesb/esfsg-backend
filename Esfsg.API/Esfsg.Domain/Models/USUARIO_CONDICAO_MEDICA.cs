namespace Esfsg.Domain.Models
{
    public class USUARIO_CONDICAO_MEDICA
    {
        public int UsuarioId { get; set; }
        public int CondicaoMedicaId { get; set; }

        public USUARIO UsuarioNavigation { get; set; }
        public CONDICAO_MEDICA CondicaoMedicaNavigation { get; set; }
    }
}
