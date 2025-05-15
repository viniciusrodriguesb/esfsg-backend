namespace Esfsg.Domain.Models
{
    public class INSCRICAO_STATUS
    {
        public int InscricaoId { get; set; }
        public int StatusId { get; set; }
        public DateTime DhInclusao { get; set; }
        public DateTime DhExclusao { get; set; }

        public INSCRICAO InscricaoNavigation { get; set; }
        public STATUS StatusNavigation { get; set; }

    }
}
