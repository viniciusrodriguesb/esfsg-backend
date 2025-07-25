﻿namespace Esfsg.Application.DTOs.Response
{
    public class InscricaoParaLiberacaoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public long Idade { get; set; }
        public string Classe { get; set; }
        public string Periodo { get; set; }
        public string FuncaoEvento { get; set; }
        public string Igreja { get; set; }
        public DadosBloqueio? UsuarioBloqueado { get; set; }
        public List<DependenteResponse>? Dependentes { get; set; }
    }

    public class DependenteResponse
    {
        public string NomeDependente { get; set; }
        public long IdadeDependente { get; set; }
    }

    public class DadosBloqueio
    {
        public bool UsuarioBloqueado { get; set; }
        public string MotivoBloqueio { get; set; }
    }

}
