using Esfsg.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Request.Relatorios
{
    public class RelatorioInscricaoRequest
    {
        [Required(ErrorMessage ="Necessário inserir o tipo de relatório.")]
        public ETipoRelatorio TipoRelatorio { get; set; }

        [Required(ErrorMessage = "É necessário enviar o evento que deseja exportar.")]
        public int IdEvento { get; set; }
    }
}
