using Microsoft.Win32;
using SAOResoForm.DBScelta;
using SAOResoForm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace SAOResoForm.Service.Repository.tool
{
    public class Tool : ITool
    {
        #region ApriDocumento
        public void ApriDocumento(Attestati attestato)
        {
            try
            {
                if (attestato == null)
                {
                    MessageBox.Show("Nessun attestato selezionato.", "Attenzione",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(attestato.LinkAttestato))
                {
                    MessageBox.Show("Nessun documento associato a questo attestato.", "Attenzione",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!File.Exists(attestato.LinkAttestato))
                {
                    MessageBox.Show($"File non trovato:\n{attestato.LinkAttestato}", "Errore",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = attestato.LinkAttestato,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore nell'apertura del documento: {ex.Message}", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region CreaCartella
        public void CreaCartella(Personale item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                string cartellaBase = SceltaDBViewModel.CaricaCartellaAttestati();
                string nomeCartella = $"{item.Cognome}_{item.Nome}_{item.Matricola}";
                string percorsoCompleto = Path.Combine(cartellaBase, nomeCartella);

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

        #region RinominaFile
        public string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione = null)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                if (string.IsNullOrEmpty(percorsoFileOriginale))
                    throw new ArgumentException("Il percorso del file originale non può essere vuoto", nameof(percorsoFileOriginale));

                if (!Directory.Exists(cartellaDestinazione))
                {
                    Directory.CreateDirectory(cartellaDestinazione);
                }

                string estensione = Path.GetExtension(percorsoFileOriginale);

                Random random = new Random();
                int numeroRandom = random.Next(10000);

                string nuovoNomeFile = $"{item.Cognome}_{item.Nome}_{item.Matricola}_{dataFine:dd-MM-yyyy}_{numeroRandom}{estensione}";
                string percorsoDestinazione = Path.Combine(cartellaDestinazione, nuovoNomeFile);

                File.Copy(percorsoFileOriginale, percorsoDestinazione, overwrite: true);

                return percorsoDestinazione;
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nella copia del file per {item.Cognome} {item.Nome}: {ex.Message}", ex);
            }
        }
        #endregion

        #region RinominaFileConNumeroFisso
        public string RinominaFile(Personale item, DateTime dataFine, string percorsoFileOriginale, string cartellaDestinazione, int numeroRandom)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                if (string.IsNullOrEmpty(percorsoFileOriginale))
                    throw new ArgumentException("Il percorso del file originale non può essere vuoto", nameof(percorsoFileOriginale));

                if (!Directory.Exists(cartellaDestinazione))
                {
                    Directory.CreateDirectory(cartellaDestinazione);
                }

                string estensione = Path.GetExtension(percorsoFileOriginale);
                string nuovoNomeFile = $"{item.Matricola}_{dataFine:dd-MM-yyyy}_{numeroRandom}{estensione}";
                string percorsoDestinazione = Path.Combine(cartellaDestinazione, nuovoNomeFile);

                File.Copy(percorsoFileOriginale, percorsoDestinazione, overwrite: true);

                return percorsoDestinazione;
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nella copia del file per {item.Cognome} {item.Nome}: {ex.Message}", ex);
            }
        }
        #endregion

        #region EsportaInCsv
        public void EsportaInCsv(IEnumerable<Attestati> dati)
        {
            try
            {
                if (dati == null || !dati.Any())
                {
                    MessageBox.Show("Nessun dato da esportare.", "Esportazione",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "File CSV (*.csv)|*.csv",
                    DefaultExt = "csv",
                    FileName = $"Attestati_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    {
                        writer.WriteLine("Dipendente;Matricola;Titolo Corso;Codice Attività;Codice Materia;Ente Formatore;Ente Certificatore;Data Inizio;Data Fine;Anno Corso;Validità Anni;Data Scadenza;Link Attestato;Attivo");

                        foreach (var attestato in dati)
                        {
                            var linea = $"{EscapaCsv(attestato.Dipendente)};" +
                                        $"{EscapaCsv(attestato.MatricolaDipendente)};" +
                                        $"{EscapaCsv(attestato.TitoloCorso)};" +
                                        $"{EscapaCsv(attestato.CodiceAttivitaFormativa)};" +
                                        $"{EscapaCsv(attestato.CodiceMateriaCorso)};" +
                                        $"{EscapaCsv(attestato.EnteFormatore)};" +
                                        $"{EscapaCsv(attestato.DenominazioneEnteCertificatore)};" +
                                        $"{EscapaCsv(attestato.DataInizioCorso)};" +
                                        $"{EscapaCsv(attestato.DataFineCorso)};" +
                                        $"{EscapaCsv(attestato.AnnoCorso)};" +
                                        $"{EscapaCsv(attestato.ValiditaAnni)};" +
                                        $"{EscapaCsv(attestato.DataScadenzaCorso)};" +
                                        $"{EscapaCsv(attestato.LinkAttestato)};" +
                                        $"{(attestato.Attivo ? "Sì" : "No")}";

                            writer.WriteLine(linea);
                        }
                    }

                    MessageBox.Show($"Esportazione completata!\nFile: {saveFileDialog.FileName}",
                        "Esportazione", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (MessageBox.Show("Vuoi aprire il file?", "Esportazione",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = saveFileDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'esportazione: {ex.Message}",
                    "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string EscapaCsv(object valore)
        {
            if (valore == null) return string.Empty;

            string testo = valore.ToString();

            if (testo.Contains(";") || testo.Contains("\"") || testo.Contains("\n"))
                return $"\"{testo.Replace("\"", "\"\"")}\"";

            return testo;
        }

        public void EsportaInCsv()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}