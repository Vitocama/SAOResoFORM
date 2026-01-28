using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RESOFORM.Dati
{
    public static class AttivitaFormativa
    {
        public static Dictionary<string, int> formazione =
            new Dictionary<string, int>
            {
                { "INFORMAZIONE", 1 },
                { "AGGIORNAMENTO", 2 },
                { "INDOTTRINAMENTO", 3 },
                { "FORMAZIONE",5},
                { "SEMINARIO", 4 }
            };
    }
}
