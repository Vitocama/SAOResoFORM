using SAOResoForm.Models;
using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

namespace SAOResoForm.Service.Repository.tool
{
    public class Tool : ITool
    {
        public void CreaCartella(Personale item)
        {
            #region creaCartella
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                string cartellaBase = @"C:\SAO";
                string nomeCartella = $"{item.Cognome}_{item.Nome}_{item.Matricola}";
                string percorsoCompleto = Path.Combine(cartellaBase, nomeCartella);

                // Crea la cartella se non esiste
                if (!Directory.Exists(percorsoCompleto))
                {
                    Directory.CreateDirectory(percorsoCompleto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nella creazione della cartella per {item.Cognome} {item.Nome}: {ex.Message}", ex);
            }
            #endregion
        }

        #region rinominaFile
        public string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione = null)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                if (string.IsNullOrEmpty(percorsoFileOriginale))
                    throw new ArgumentException("Il percorso del file originale non può essere vuoto", nameof(percorsoFileOriginale));

                // ✅ CREA LA CARTELLA DI DESTINAZIONE SE NON ESISTE
                if (!Directory.Exists(cartellaDestinazione))
                {
                    Directory.CreateDirectory(cartellaDestinazione);
                }

                // Estensione file originale
                string estensione = Path.GetExtension(percorsoFileOriginale);

                // Genera numero random
                Random random = new Random();
                int NumeroRandom = random.Next(10000);

                // Nuovo nome file: Cognome_Nome_Matricola_DataFine_Random.extn
                string nuovoNomeFile = $"{item.Cognome}_{item.Nome}_{item.Matricola}_{dataFine:dd-MM-yyyy}_{NumeroRandom}{estensione}";

                // Percorso completo destinazione
                string percorsoDestinazione = Path.Combine(cartellaDestinazione, nuovoNomeFile);

                // Copia il file (sovrascrive se esiste)
                File.Copy(percorsoFileOriginale, percorsoDestinazione, overwrite: true);

                return percorsoDestinazione;
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nella copia del file per {item.Cognome} {item.Nome}: {ex.Message}", ex);
            }
        }
        #endregion

        #region rinominaFileConNumeroFisso
        /// <summary>
        /// Rinomina e copia un file usando un numero random predefinito (per più utenti)
        /// </summary>
        public string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione, int numeroRandom)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                if (string.IsNullOrEmpty(percorsoFileOriginale))
                    throw new ArgumentException("Il percorso del file originale non può essere vuoto", nameof(percorsoFileOriginale));

                // ✅ CREA LA CARTELLA DI DESTINAZIONE SE NON ESISTE
                if (!Directory.Exists(cartellaDestinazione))
                {
                    Directory.CreateDirectory(cartellaDestinazione);
                }

                // Estensione file originale
                string estensione = Path.GetExtension(percorsoFileOriginale);

                // Nuovo nome file: Matricola_DataFine_NumeroFisso.extn
                string nuovoNomeFile = $"{item.Matricola}_{dataFine:dd-MM-yyyy}_{numeroRandom}{estensione}";

                // Percorso completo destinazione
                string percorsoDestinazione = Path.Combine(cartellaDestinazione, nuovoNomeFile);

                // Copia il file (sovrascrive se esiste)
                File.Copy(percorsoFileOriginale, percorsoDestinazione, overwrite: true);

                return percorsoDestinazione;
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nella copia del file per {item.Cognome} {item.Nome}: {ex.Message}", ex);
            }
        }
        #endregion
    }
}