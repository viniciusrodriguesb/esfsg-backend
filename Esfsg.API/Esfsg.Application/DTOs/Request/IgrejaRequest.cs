using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class IgrejaRequest
    {
        [Required]
        public int IdPastor {  get; set; }

        [Required]
        public int IdRegiao { get; set; }

        [Required]
        public string Nome { get; set; }
    }
}
