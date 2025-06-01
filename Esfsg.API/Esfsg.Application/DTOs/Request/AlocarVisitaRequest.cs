using Esfsg.Application.Enums;

namespace Esfsg.Application.DTOs.Request
{
    public class AlocarVisitaRequest
    {
        public FuncaoVisitaEnum Funcao { get; set; }
        public int IdInscricao { get; set; }
        public int IdVisita { get; set; }
    }
}
