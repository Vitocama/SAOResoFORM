using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESOFORM.Dati
{
    internal class RepartiCodice
    {
      public  Dictionary<string, int> reparti = new Dictionary<string, int>
{
    { "DA",  1 },
    { "VDA", 2 },
    { "RAM", 3 },
    { "RMN", 4 },
    { "RSTA", 5 },
    { "RCLE", 6 }
};
    }
}
