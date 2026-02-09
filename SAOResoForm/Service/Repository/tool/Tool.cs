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
        }
        #endregion

        #region rinominaFile

        public string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione = null)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                if (string.IsNullOrEmpty(percorsoFileOriginale))
                    throw new ArgumentException("Il percorso del file originale non può essere vuoto", nameof(percorsoFileOriginale));

                bool fileCreato = false;

                // Crea la cartella se non esiste
                Directory.CreateDirectory(cartellaDestinazione);

                // Estensione file originale
                string estensione = Path.GetExtension(percorsoFileOriginale);

                Random random = new Random();

                int NumeroRandom = random.Next(10000);

                // Nuovo nome file: Cognome_Nome_Matricola_DataFine.extn
                string nuovoNomeFile = $"{item.Cognome}_{item.Nome}_{item.Matricola}_{dataFine:dd-MM-yyyy}_{NumeroRandom}{estensione}";

                // Percorso completo destinazione
                string percorsoDestinazione = Path.Combine(cartellaDestinazione, nuovoNomeFile);

                // Copia il file (sovrascrive se esiste)
                File.Copy(percorsoFileOriginale, percorsoDestinazione, overwrite: true);

                // Se il file è stato creato, mostra messaggio
                if (fileCreato)
                {
                    System.Windows.MessageBox.Show(
                        $"ATTENZIONE: Il file originale non esisteva ed è stato creato vuoto:\n{percorsoFileOriginale}\n\n" +
                        $"Il file è stato copiato in:\n{percorsoDestinazione}",
                        "File Creato",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Warning
                    );
                }

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
