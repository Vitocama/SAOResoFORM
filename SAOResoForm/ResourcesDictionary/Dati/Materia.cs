using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESOFORM.Dati
{
    public static class Materia
    {
        public static Dictionary<string, List<string>> materiafb =
           new Dictionary<string, List<string>>
           {
                {
                    "INFORMAZIONE",
                    new List<string>
                    {
                        "SICUREZZA SUL LAVORO",
                        "TECNICO",
                        "AMMINISTRATIVO",
                        "QUALITA'",
                        "ANTICORRUZIONE"
                    }
                },
                {
                    "FORMAZIONE",
                    new List<string>
                    {
                        "SICUREZZA SUL LAVORO",
                        "TECNICO",
                        "AMMINISTRATIVO",
                        "QUALITA'",
                        "AMBIENTE",
                        "ATLANTIS",
                        "ANTICORRUZIONE"
                    }
                },
                {
                    "AGGIORNAMENTO",
                    new List<string>
                    {
                        "SICUREZZA SUL LAVORO"
                    }
                },
                {
                    "INDOTTRINAMENTO",
                    new List<string>
                    {
                        "TECNICO"
                    }
                },
                {
                    "SEMINARIO",
                    new List<string>
                    {
                        "TECNICO",
                        "ALTRO"
                    }
                }
           };
    }


}

