﻿namespace Esfsg.Domain.Models
{
    public class EMAIL_BODY
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int? IdStatus { get; set; }

        #region Navigations
        public STATUS? StatusNavigation { get; set; }
        #endregion

    }
}
