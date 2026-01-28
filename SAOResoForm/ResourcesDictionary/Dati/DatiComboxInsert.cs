using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace SAOResoForm.ResourcesDictionary.Dati
{
    public class DatiComboxInsert
    {
      public  ObservableCollection<string> stato_di_servizio = new ObservableCollection<string>() {
            "ASPETTATIVA",
            "CESSATO",
            "COMANDO",
            "DISTACCO SINDACALE",
            "IN SERVIZIO",
            "LUNGA DEGENZA",
            "TEMPO ASSEGNATO",
            "TRASFERITO",
            "TURNISTA",
            "IN FORZA",
            "TEMP. ASSEGN"
            };
      public  ObservableCollection<string> milciv = new ObservableCollection<string>()
            {
                "MIL",
                "CIV"
            };
      public  ObservableCollection<string> incarico = new ObservableCollection<string>() {
            "DA",
            "VDA",
            "CAPO REPARTO",
            "CAPO SEZIONE",
            "CAPO NUCLEO",
            "CAPO UFFICIO",
            "CAPO SERVIZIO",
            "ADDETTO",
            "CAPO UNITA",
            "RSPP",
            "ASPP",
            "RSPP",
            "RSAQ",
            "AMMINISTRATORE"
            };
    }
}