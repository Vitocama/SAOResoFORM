using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAOResoForm.Models
{
    public partial class TipoFormazione
    {
        public string Id { get; set; }
        public string CodiceAttivitaFormativa { get; set; }
        public string AttivitaFormativa { get; set; }
    }
}
