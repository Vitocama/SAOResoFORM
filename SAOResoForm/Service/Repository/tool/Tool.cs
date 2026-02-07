using SAOResoForm.Models;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SAOResoForm.Service.Repository.tool
{
    public class Tool : ITool
    {
        // ========================
        // CREA CARTELLA PERSONALE
        // ========================
        public void CreaCartella(Personale item)
        {
            try
            {
                // Percorso base
                string percorsoBase = @"C:\SAO";

                // Verifica se la cartella base esiste, altrimenti creala
                if (!Directory.Exists(percorsoBase))
                {
                    Directory.CreateDirectory(percorsoBase);
                }

                // Crea il nome della cartella: "Cognome_Nome_Matricola"
                string nomeCartella = $"{item.Cognome}_{item.Nome}_{item.Matricola}";

                // Rimuovi caratteri non validi dal nome della cartella
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    nomeCartella = nomeCartella.Replace(c.ToString(), "");
                }

                // Percorso completo della cartella
                string percorsoCompleto = Path.Combine(percorsoBase, nomeCartella);

                // Verifica se la cartella esiste già
                if (Directory.Exists(percorsoCompleto))
                {
                    MessageBox.Show($"La cartella esiste già:\n{percorsoCompleto}",
                        "Informazione",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                // Crea la cartella
                Directory.CreateDirectory(percorsoCompleto);

                MessageBox.Show($"Cartella creata con successo:\n{percorsoCompleto}",
                    "Successo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Permessi insufficienti per creare la cartella in C:\\SAO.\n" +
                                "Esegui l'applicazione come Amministratore.",
                    "Errore Permessi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la creazione della cartella:\n{ex.Message}",
                    "Errore",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // ========================
        // RINOMINA E SPOSTA FILE
        // ========================
        public string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale)
        {
            try
            {
                // Se non c'è un file, restituisci null
                if (string.IsNullOrEmpty(percorsoFileOriginale))
                {
                    return null;
                }

                // Verifica che il file esista
                if (!File.Exists(percorsoFileOriginale))
                {
                    throw new FileNotFoundException("Il file specificato non esiste.", percorsoFileOriginale);
                }

                // Crea il percorso della cartella personale: C:\SAO\Attestati\Cognome_Nome_Matricola
                string cartellaPersonale = Path.Combine(
                    @"C:\SAO\Attestati",
                    $"{item.Cognome}_{item.Nome}_{item.Matricola}"
                );

                // Crea la cartella se non esiste
                if (!Directory.Exists(cartellaPersonale))
                {
                    Directory.CreateDirectory(cartellaPersonale);
                }

                // Ottieni l'estensione del file originale
                string estensione = Path.GetExtension(percorsoFileOriginale);

                // Genera un numero random di 5 cifre
                Random random = new Random();
                int numeroRandom = random.Next(10000, 99999);

                // Formatta la data di fine (esempio: 30012025)
                string dataFineFormattata = dataFine.ToString("ddMMyyyy");

                // Crea il nuovo nome file con data fine e numero random
                string nuovoNomeFile = $"{item.Cognome}_{item.Nome}_{item.Matricola}_{dataFineFormattata}_{numeroRandom}{estensione}";

                // Crea il percorso completo del nuovo file
                string nuovoPercorsoCompleto = Path.Combine(cartellaPersonale, nuovoNomeFile);

                // Se esiste già un file con lo stesso nome (molto improbabile), aggiungi timestamp
                if (File.Exists(nuovoPercorsoCompleto))
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    nuovoNomeFile = $"{item.Cognome}_{item.Nome}_{item.Matricola}_{dataFineFormattata}_{numeroRandom}_{timestamp}{estensione}";
                    nuovoPercorsoCompleto = Path.Combine(cartellaPersonale, nuovoNomeFile);
                }

                // Copia il file (mantiene l'originale)
                File.Copy(percorsoFileOriginale, nuovoPercorsoCompleto);

                // Restituisci il nuovo percorso
                return nuovoPercorsoCompleto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante la copia del file: {ex.Message}", ex);
            }
        }
    }
}