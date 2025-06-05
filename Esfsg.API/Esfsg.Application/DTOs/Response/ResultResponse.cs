﻿namespace Esfsg.Application.DTOs.Response
{
    public class ResultResponse<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Dados { get; set; }
    }
}
