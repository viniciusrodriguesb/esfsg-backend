using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request
{
    public class InscricaoEventoResquest
    {
        [Range(1, int.MaxValue, ErrorMessage = "IdEvento deve ser um valor positivo")]
        public int IdEvento { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "IdUsuario deve ser um valor positivo")]
        public int IdUsuario { get; set; }
    }
}
