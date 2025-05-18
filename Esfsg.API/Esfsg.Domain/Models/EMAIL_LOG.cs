namespace Esfsg.Domain.Models
{
    public class EMAIL_LOG
    {
        public int Id { get; set; }
        public int IdInscricao { get; set; }
        public int IdStatus { get; set; }
        public bool Enviado { get; set; }
        public string Observacoes { get; set; }
        public DateTime DhEnvio { get; set; }
    }
}
