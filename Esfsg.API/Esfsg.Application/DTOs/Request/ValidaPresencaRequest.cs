﻿namespace Esfsg.Application.DTOs.Request
{
    public class ValidaPresencaRequest
    {
        public int? IdCheckIn { get; set; }
        public string? QrCode { get; set; }
        public bool Presenca { get; set; }
    }
}
