using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAOResoForm.Models
{
    public partial class Personale
    {
        public long Id { get; set; }
        public string GradoQualifica { get; set; }
        public string CategoriaProfilo { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string MilCiv { get; set; }
        public string Matricola { get; set; }
        public string CodReparto { get; set; }
        public string CodSezione { get; set; }
        public string CodNucleo { get; set; }
        public long? CodUfficio { get; set; }
        public string Incarico { get; set; }
        public string StatoServizio { get; set; }
        public string Annotazioni { get; set; }
    }
}
