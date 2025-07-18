﻿namespace Esfsg.Application.DTOs.Response
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string Nascimento { get; set; }
        public string Pcd { get; set; }
        public bool PossuiDons { get; set; }
        public bool UsuarioBloqueado { get; set; }
        public TabelaDominioResponse TipoUsuario { get; set; }
        public TabelaDominioResponse Classe { get; set; }
        public TabelaDominioResponse Igreja { get; set; }
        public List<string> CondicoesMedica { get; set; }
        public List<string> FuncoesIgreja { get; set; }
        public List<string> Instrumentos { get; set; }
    }

    public class DadosUsuario
    {
        public int? TipoUsuario { get; set; }
        public int PastorId { get; set; }
    }
}
