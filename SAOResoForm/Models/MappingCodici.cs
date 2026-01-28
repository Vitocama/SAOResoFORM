using System.Collections.Generic;
using System.Linq;

namespace SAOResoForm.Dati
{
    public static class MappingCodici
    {
        // Mapping Reparto -> Codice
        public static Dictionary<string, long> RepartiCodici = new Dictionary<string, long>
        {
            { "DA", 1 },
            { "RAM", 2 },
            { "RCLE", 3 },
            { "RMN", 4 },
            { "RSTA", 5 },
            { "VDA", 6 }
        };

        // Mapping Sezione -> Codice
        public static Dictionary<string, long> SezioniCodici = new Dictionary<string, long>
        {
            { "-", 0 },
            { "PCN", 1 },
            { "CONSULENZA GIURIDICA", 2 },
            { "SPP", 3 },
            { "UCS", 4 },
            { "URP", 5 },
            { "ATTIVITA NEGOZIALI", 6 },
            { "GESTIONE FINANZIARIA", 7 },
            { "GESTIONE PATRIMONIALE", 8 },
            { "SEGRETERIA RAM", 9 },
            { "PIANIFICAZIONE ESECUTIVA", 10 },
            { "SEGRETERIA RCLE", 11 },
            { "STUDI ED ESPERIENZE", 12 },
            { "VERIFICA DI CONFORMITA", 13 },
            { "BACINI", 14 },
            { "SEGRETERIA RMN", 15 },
            { "SSC", 16 },
            { "SSN", 17 },
            { "INFRASTRUTTURE", 18 },
            { "SEGRETERIA RSTA", 19 },
            { "SERVIZI ARSENALE", 20 },
            { "SUPPORTI INFORMATICI", 21 },
            { "SAG", 22 },
            { "SANITARIO", 23 },
            { "SAO", 24 },
            { "SAQ", 25 },
            { "SEGRETERIA VDA", 26 },
            { "SPE", 27 }
        };

        // Mapping Nucleo -> Codice
        public static Dictionary<string, long> NucleiCodici = new Dictionary<string, long>
        {
            { "-", 0 },
            { "AMBIENTE", 1 },
            { "SICUREZZA SUL LAVORO", 2 },
            { "MENSA", 3 },
            { "PROTOCOLLO E ARCHIVIO", 4 },
            { "PERSONALE MILITARE", 5 },
            { "PERSONALE CIVILE", 6 },
            { "AM", 7 },
            { "IEB", 8 },
            { "MA", 9 },
            { "SCAFO E ALLESTIMENTI", 10 },
            { "SDL E CND", 11 },
            { "SISTEMI ELETTROMECCANICI ARMA E SENSORI", 12 },
            { "SISTEMI ELETTRONICI E TLC", 13 },
            { "GO53", 14 },
            { "GO58", 15 },
            { "GO60", 16 },
            { "MATERIALI SPECIALI", 17 },
            { "PIANIFICAZIONE", 18 },
            { "CONTROLLO", 19 },
            { "STRUTTURE", 20 },
            { "IMPIANTI", 21 },
            { "TRASPORTI E SOLLEVAMENTI", 22 },
            { "RETI E IMPIANTI ELETTRICI", 23 },
            { "SUPPORTI E MANUTENZIONE EDILE", 24 },
            { "GESTIONE RISORSE INFORMATICHE", 25 },
            { "GESTIONE SOFTWARE", 26 },
            { "PIANIFICAZIONE, CONTROLLO E PERMUTE", 27 },
            { "NAVALE ED ALIENAZIONI", 28 },
            { "TERRESTRE", 29 },
            { "ECONOMIE", 30 },
            { "CONTRATTI E PERMUTE", 31 },
            { "ESECUZIONE E LIQUIDAZIONE CONTRATTI ED ECONOMIE", 32 },
            { "ESECUZIONE E LIQUIDAZIONE COMPETENZE AL PERSONALE", 33 },
            { "GESTIONE FINANZIARIA", 34 },
            { "MAGAZZINI SUPPORTO/TRANSITO/FUORI USO", 35 },
            { "SETTORE ECONOMATO", 36 },
            { "SETTORE SALA RICEZIONE", 37 },
            { "ECONOMATO E SALA RICEZIONE", 38 }
        };

        // Metodi helper per ottenere il codice (ritorna null se non trovato)
        public static long? GetCodiceReparto(string nomeReparto)
        {
            return string.IsNullOrWhiteSpace(nomeReparto) || !RepartiCodici.ContainsKey(nomeReparto)
                ? (long?)null
                : RepartiCodici[nomeReparto];
        }

        public static long? GetCodiceSezione(string nomeSezione)
        {
            if (string.IsNullOrWhiteSpace(nomeSezione) || nomeSezione == "-")
                return null;

            return SezioniCodici.ContainsKey(nomeSezione)
                ? SezioniCodici[nomeSezione]
                : (long?)null;
        }

        public static long? GetCodiceNucleo(string nomeNucleo)
        {
            if (string.IsNullOrWhiteSpace(nomeNucleo) || nomeNucleo == "-")
                return null;

            return NucleiCodici.ContainsKey(nomeNucleo)
                ? NucleiCodici[nomeNucleo]
                : (long?)null;
        }
    }
}