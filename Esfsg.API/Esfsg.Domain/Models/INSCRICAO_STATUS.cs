namespace Esfsg.Domain.Models
{
    public class INSCRICAO_STATUS
    {
        public int InscricaoId { get; set; }
        public int StatusId { get; set; }
        public DateTime DhInclusao { get; set; }
        public DateTime? DhExclusao { get; set; }

        #region Navigations
        public INSCRICAO InscricaoNavigation { get; set; } = new INSCRICAO();
        public STATUS StatusNavigation { get; set; } = new STATUS(); 
        #endregion

    }
}
