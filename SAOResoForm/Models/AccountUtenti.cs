using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAOResoForm.Models
{
    public partial class AccountUtenti
    {
        public int Id { get; set; }
        public string Utente { get; set; }
        public string Password { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Incarico { get; set; }
        public string Amministratore { get; set; }
        public string Consultazione { get; set; }
        public string Uuoo { get; set; }
        public string Ruolo { get; set; }
        public string Matricola { get; set; }
    }
}
