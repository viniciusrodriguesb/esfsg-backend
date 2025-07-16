using System.ComponentModel.DataAnnotations;

namespace Esfsg.Application.DTOs.Response.Relatorios
{
    public class RelatorioInscricaoResponse
    {

        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [Display(Name ="CPF")]
        public string Cpf { get; set; }

        [Display(Name ="Telefone")]
        public string Telefone { get; set; }

        [Display(Name ="Classe")]
        public string Classe { get; set; }

        [Display(Name ="Igreja")]
        public string Igreja { get; set; }

        [Display(Name ="Período")]
        public string Periodo { get; set; }

        [Display(Name ="Função no Evento")]
        public string FuncaoEvento { get; set; }

        [Display(Name ="Data da Inscrição")]
        public string DhInscricao { get; set; }

        [Display(Name ="Status da Inscrição")]
        public string StatusInscricao { get; set; }

        [Display(Name ="Presença")]
        public string Presenca { get; set; }
    }
}
