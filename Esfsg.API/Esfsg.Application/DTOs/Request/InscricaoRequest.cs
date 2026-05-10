using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class InscricaoRequest
    {
        [Required(ErrorMessage = "CPF é obrigatório")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "Período é obrigatório")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Período deve ter entre 1 e 50 caracteres")]
        public string Periodo { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "IdFuncaoEvento deve ser um valor positivo")]
        public int IdFuncaoEvento { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "IdEvento deve ser um valor positivo")]
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "Dados de visita são obrigatórios")]
        public VisitaInscricaoRequest Visita { get; set; } = new VisitaInscricaoRequest();

        public UsuarioRequest? Usuario { get; set; }

        public List<MenorRequest>? InscricaoMenor { get; set; }

        public IgrejaInscricaoRequest? Igreja { get; set; }
    }

    public class VisitaInscricaoRequest
    {
        public bool Visita { get; set; }

        [Range(0, 10, ErrorMessage = "Vagas deve estar entre 0 e 10")]
        public int? Vagas { get; set; }

        public bool? Carro { get; set; }
    }

    public class IgrejaInscricaoRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "IdRegião deve ser um valor positivo")]
        public int IdRegiao { get; set; }

        [Required(ErrorMessage = "Nome da igreja é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Nome da igreja deve ter entre 3 e 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome do pastor é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Nome do pastor deve ter entre 3 e 150 caracteres")]
        public string Pastor { get; set; } = string.Empty;
    }

    public class MenorRequest
    {
        [Range(1, 7, ErrorMessage = "Idade do menor deve estar entre 1 e 7 anos")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "Nome do menor é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Nome do menor deve ter entre 3 e 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "IdCondicaoMedica deve ser um valor positivo ou nulo")]
        public int? IdCondicaoMedica { get; set; }
    }
}
