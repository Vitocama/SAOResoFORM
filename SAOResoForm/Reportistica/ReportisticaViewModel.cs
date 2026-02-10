using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32; // ✅ AGGIUNGI QUESTA
using SAOResoForm.Models;
using SAOResoForm.Repositories;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.Reportistica
{
    public class ReportisticaViewModel : ViewModelBase
    {
        #region Fields

        private readonly IRepositoryAttestato _repositoryAttestato;

        private ObservableCollection<Attestati> _bdAttestati;
        private ObservableCollection<Attestati> _filteredBDAttestati;
        private string _filtroRicerca;

        #endregion

        #region Properties

        public ObservableCollection<Attestati> BDAttestati
        {
            get => _bdAttestati;
            set => Set(ref _bdAttestati, value);
        }

        public ObservableCollection<Attestati> FilteredBDAttestati
        {
            get => _filteredBDAttestati;
            set => Set(ref _filteredBDAttestati, value);
        }

        public string FiltroRicerca
        {
            get => _filtroRicerca;
            set
            {
                if (Set(ref _filtroRicerca, value))
                {
                    ApplicaFiltro();
                }
            }
        }

        #endregion

        #region Command

        private ICommand _esportaCommand;
        public ICommand EsportaCommand =>  new RelayCommand(EsportaInCsv);

        #endregion

        #region Constructor

        public ReportisticaViewModel(IRepositoryAttestato repository)
        {
            _repositoryAttestato = repository ?? throw new ArgumentNullException(nameof(repository));

            BDAttestati = new ObservableCollection<Attestati>();
            FilteredBDAttestati = new ObservableCollection<Attestati>();

            CaricaDati();
        }

        #endregion

        #region Methods

        private void CaricaDati()
        {
            try
            {
                var attestati = _repositoryAttestato.GetAll();

                BDAttestati.Clear();
                foreach (var attestato in attestati)
                {
                    BDAttestati.Add(attestato);
                }

                ApplicaFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Errore durante il caricamento dei dati: {ex.Message}",
                    "Errore",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void EsportaInCsv()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "File CSV (*.csv)|*.csv",
                    DefaultExt = "csv",
                    FileName = $"Attestati_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    // ✅ USA FilteredBDAttestati invece di FiltraAttestati
                    var datiDaEsportare = FilteredBDAttestati?.ToList() ?? BDAttestati.ToList();

                    if (!datiDaEsportare.Any())
                    {
                        MessageBox.Show("Nessun dato da esportare.", "Esportazione",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    using (var writer = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    {
                        // Intestazione
                        writer.WriteLine("Matricola;Titolo Corso;Codice Attività;Codice Materia;Ente Formatore;Ente Certificatore;Data Inizio;Data Fine;Anno Corso;Validità Anni;Data Scadenza;Link Attestato");

                        // Dati
                        foreach (var attestato in datiDaEsportare)
                        {
                            var linea = $"{EscapaCsv(attestato.MatricolaDipendente)};" +
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
                                        $"{EscapaCsv(attestato.LinkAttestato)}";

                            writer.WriteLine(linea);
                        }
                    }

                    MessageBox.Show($"Esportazione completata con successo!\nFile salvato in: {saveFileDialog.FileName}",
                        "Esportazione", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Opzionale: apri il file
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

        private string EscapaCsv(string valore)
        {
            if (string.IsNullOrEmpty(valore))
                return "";

            if (valore.Contains(";") || valore.Contains(",") || valore.Contains("\"") || valore.Contains("\n"))
            {
                valore = valore.Replace("\"", "\"\"");
                return $"\"{valore}\"";
            }

            return valore;
        }

        private void ApplicaFiltro()
        {
            FilteredBDAttestati.Clear();

            if (string.IsNullOrWhiteSpace(FiltroRicerca))
            {
                foreach (var attestato in BDAttestati)
                {
                    FilteredBDAttestati.Add(attestato);
                }
            }
            else
            {
                var filtro = FiltroRicerca.ToLower();

                var risultati = BDAttestati.Where(a =>
                    a.Id.ToString().Contains(filtro) ||
                    (!string.IsNullOrEmpty(a.MatricolaDipendente) && a.MatricolaDipendente.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.TitoloCorso) && a.TitoloCorso.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.CodiceAttivitaFormativa) && a.CodiceAttivitaFormativa.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.CodiceMateriaCorso) && a.CodiceMateriaCorso.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.EnteFormatore) && a.EnteFormatore.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.DenominazioneEnteCertificatore) && a.DenominazioneEnteCertificatore.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.DataInizioCorso) && a.DataInizioCorso.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.DataFineCorso) && a.DataFineCorso.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.AnnoCorso) && a.AnnoCorso.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.ValiditaAnni) && a.ValiditaAnni.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.DataScadenzaCorso) && a.DataScadenzaCorso.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.LinkAttestato) && a.LinkAttestato.ToLower().Contains(filtro))
                );

                foreach (var attestato in risultati)
                {
                    FilteredBDAttestati.Add(attestato);
                }
            }
        }

        #endregion
    }
}