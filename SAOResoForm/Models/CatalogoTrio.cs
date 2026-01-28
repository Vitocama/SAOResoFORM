using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAOResoForm.Models
{
    public partial class CatalogoTrio
    {
        public string Id { get; set; }
        public string Codice { get; set; }
        public string Titolo { get; set; }
        public string MacroareaTematica { get; set; }
        public string AreaTematica { get; set; }
        public string Durata { get; set; }
        public string CodiceAttivitaFormativa { get; set; }
        public string CodiceMateriaCorso { get; set; }
        public string CodiceTestoBreve { get; set; }
    }
}
