using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAOResoForm.Models
{
    public partial class Corsi
    {
        public byte[] Id { get; set; }
        public string Matricola { get; set; }
        public string DescrizioneCorso { get; set; }
        public string DataInizioCorso { get; set; }
        public string DataFineCorso { get; set; }
        public string Incarichi { get; set; }
        public string FileInput { get; set; }
    }
}
