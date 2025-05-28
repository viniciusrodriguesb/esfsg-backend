using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class InscricaoRequest
    {
        [StringLength(14)]
        public string Cpf { get; set; }
        public string Periodo { get; set; }
        public int IdFuncaoEvento { get; set; }
        public int IdEvento { get; set; }
        public VisitaInscricaoRequest Visita { get; set; }
        public UsuarioRequest? Usuario { get; set; }
        public List<MenorRequest>? InscricaoMenor { get; set; }
        public IgrejaInscricaoRequest? Igreja { get; set; }
    }

    public class VisitaInscricaoRequest
    {
        public bool Visita { get; set; }
        public int? Vagas { get; set; }
        public bool? Carro { get; set; }
    }
    public class IgrejaInscricaoRequest
    {
        public int IdRegiao { get; set; }
        public string Nome { get; set; }
        public string Pastor { get; set; }
    }

    public class MenorRequest
    {
        public int Idade { get; set; }
        public int IdCondicaoMedica { get; set; }
    }
}
