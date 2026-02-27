using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOResoForm.ResourcesDictionary.Dati
{
    public class RepartiSezioniNucleoKeyValue
    {
        public Dictionary<string, List<string>> Data1 = new Dictionary<string, List<string>>()
{
    { "DA",   new List<string> { "-", "PCN", "CONSULENTE GIURIDICO", "SEGRETERIA DA", "SPP", "UCS", "URP" } },
    { "RAM",  new List<string> { "-", "ATTIVITÀ NEGOZIALI", "GESTIONE FINANZIARIA", "GESTIONE PATRIMONIALE", "SEGRETERIA RAM" } },
    { "RCLE", new List<string> { "-", "PIANIFICAZIONE ESECUTIVA", "SEGRETERIA RCLE", "STUDI ED ESPERIENZE", "VERIFICA DI CONFORMITÀ" } },
    { "RMN",  new List<string> { "-", "BACINI", "PROGRAMMI NAVALI", "SEGRETERIA RMN", "SSC", "SSN" } },
    { "RSTA", new List<string> { "-", "INFRASTRUTTURE", "SEGRETERIA RSTA", "SERVIZI ARSENALE", "SUPPORTI INFORMATICI" } },
    { "VDA",  new List<string> { "-", "SAG", "SANITARIO", "SAO", "SAQ", "SEGRETERIA VDA", "SPE" } },
};

        public static Dictionary<string, List<string>> Data2 => new Dictionary<string, List<string>>
{
    { "DA",   new List<string> { "-", "PCN", "CONSULENTE GIURIDICO", "SEGRETERIA DA", "SPP", "UCS", "URP" } },
    { "RAM",  new List<string> { "-", "ATTIVITÀ NEGOZIALI", "GESTIONE FINANZIARIA", "GESTIONE PATRIMONIALE", "SEGRETERIA RAM" } },
    { "RCLE", new List<string> { "-", "PIANIFICAZIONE ESECUTIVA", "SEGRETERIA RCLE", "STUDI ED ESPERIENZE", "VERIFICA DI CONFORMITÀ" } },
    { "RMN",  new List<string> { "-", "BACINI", "PROGRAMMI NAVALI", "SEGRETERIA RMN", "SSC", "SSN" } },
    { "RSTA", new List<string> { "-", "INFRASTRUTTURE", "SEGRETERIA RSTA", "SERVIZI ARSENALE", "SUPPORTI INFORMATICI" } },
    { "VDA",  new List<string> { "-", "SAG", "SANITARIO", "SAO", "SAQ", "SEGRETERIA VDA", "SPE" } },
};

        public static Dictionary<string, List<string>> TerzoLivello => new Dictionary<string, List<string>>
{
    { "-",                          new List<string> { "-" } },
    { "PCN",                        new List<string> { "-" } },
    { "SEGRETERIA DA",              new List<string> { "-" } },  // AGGIUNTO
    { "SEGRETERIA RAM",             new List<string> { "-" } },
    { "SEGRETERIA RSTA",            new List<string> { "-" } },
    { "SEGRETERIA RMN",             new List<string> { "-" } },
    { "CONSULENTE GIURIDICO",       new List<string> { "-" } },
    { "SEGRETERIA RCLE",            new List<string> { "-" } },
    { "VERIFICA DI CONFORMITÀ",     new List<string> { "-" } },
    { "UCS",                        new List<string> { "-" } },
    { "SPP",                        new List<string> { "-", "AMBIENTE", "SICUREZZA SUL LAVORO" } },
    { "URP",                        new List<string> { "-" } },
    { "SANITARIO",                  new List<string> { "-" } },
    { "SAG",                        new List<string> { "-", "MENSA", "PROTOCOLLO E ARCHIVIO" } },
    { "SAO",                        new List<string> { "-" } },
    { "SPE",                        new List<string> { "-", "PERSONALE MILITARE", "PERSONALE CIVILE" } },
    { "SAQ",                        new List<string> { "-" } },
    { "SSN",                        new List<string> { "-", "AM", "IEB", "MA", "SCAFO E ALLESTIMENTI", "SDL E CND" } },
    { "SSC",                        new List<string> { "-", "SISTEMI ELETTROMECCANICI ARMA E SENSORI", "SISTEMI ELETTRONICI E TLC" } },
    { "BACINI",                     new List<string> { "-", "GO53", "GO58", "GO60" } },
    { "PROGRAMMI NAVALI",           new List<string> { "-", "MATERIALI SPECIALI", "PIANIFICAZIONE", "CONTROLLO" } },  // AGGIUNTO
    { "STUDI ED ESPERIENZE",        new List<string> { "-" } },
    { "SEGRETERIA VDA",             new List<string> { "-" } },
    { "INFRASTRUTTURE",             new List<string> { "-", "STRUTTURE", "IMPIANTI" } },
    { "SERVIZI ARSENALE",           new List<string> { "-", "TRASPORTI E SOLLEVAMENTI", "RETI E IMPIANTI ELETTRICI", "SUPPORTI E MANUTENZIONE EDILE" } },
    { "SUPPORTI INFORMATICI",       new List<string> { "-", "GESTIONE RISORSE INFORMATICHE", "GESTIONE SOFTWARE" } },
    { "PIANIFICAZIONE ESECUTIVA",   new List<string> { "-", "PIANIFICAZIONE, CONTROLLO E PERMUTE", "NAVALE ED ALIENAZIONI", "TERRESTRE" } },
    { "VERIFICHE DI CONFORMITÀ",    new List<string> { "-" } },
    { "ATTIVITÀ NEGOZIALI",         new List<string> { "-", "ECONOMIE", "CONTRATTI E PERMUTE" } },
    { "GESTIONE FINANZIARIA",       new List<string> { "-", "ESECUZIONE E LIQUIDAZIONE CONTRATTI ED ECONOMIE", "ESECUZIONE E LIQUIDAZIONE COMPETENZE AL PERSONALE", "GESTIONE FINANZIARIA" } },
    { "GESTIONE PATRIMONIALE",      new List<string> { "-", "MAGAZZINI SUPPORTO/TRANSITO/FUORI USO", "SETTORE ECONOMATO", "SETTORE SALA RICEZIONE", "ECONOMATO E SALA RICEZIONE" } }
};
    }
}