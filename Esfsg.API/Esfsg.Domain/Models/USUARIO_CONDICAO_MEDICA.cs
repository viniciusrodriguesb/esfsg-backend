namespace Esfsg.Domain.Models
{
    public class USUARIO_CONDICAO_MEDICA
    {
        public int UsuarioId { get; set; }
        public int CondicaoMedicaId { get; set; }

        #region Navigations
        public USUARIO UsuarioNavigation { get; set; } = new USUARIO();
        public CONDICAO_MEDICA CondicaoMedicaNavigation { get; set; } = new CONDICAO_MEDICA(); 
        #endregion

    }
}
