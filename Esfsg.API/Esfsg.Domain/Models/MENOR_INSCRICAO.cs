﻿namespace Esfsg.Domain.Models
{
    public class MENOR_INSCRICAO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public int IdInscricao { get; set; }
        public int? IdCondicaoMedica { get; set; }

        #region Navigations

        public INSCRICAO InscricaoNavigation { get; set; }
        public CONDICAO_MEDICA CondicaoMedicaNavigation { get; set; }

        #endregion

    }
}
