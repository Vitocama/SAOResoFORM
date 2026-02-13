using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using SAOResoForm.Models;
using SAOResoForm.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        // ✅ Lista per tracciare gli attestati modificati
        private HashSet<int> _attestatiModificati;

        // ✅ Filtro attivo corrente
        private FiltroAttivoEnum _filtroAttivoCorrente = FiltroAttivoEnum.Tutti;

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

        public ICommand EsportaCommand => new RelayCommand(EsportaInCsv);
        public ICommand SalvaCommand => new RelayCommand(SalvaModifiche, () => _attestatiModificati.Count > 0);
        public ICommand AggiornaCommand => new RelayCommand(CaricaDati);
        public ICommand ResetFiltroCommand => new RelayCommand(ResetFiltro);

        // ✅ Nuovi comandi per filtri Attivo
        public ICommand MostraTuttiCommand => new RelayCommand(() => ImpostaFiltroAttivo(FiltroAttivoEnum.Tutti));
        public ICommand MostraAttiviCommand => new RelayCommand(() => ImpostaFiltroAttivo(FiltroAttivoEnum.SoloAttivi));
        public ICommand MostraNonAttiviCommand => new RelayCommand(() => ImpostaFiltroAttivo(FiltroAttivoEnum.SoloNonAttivi));

        #endregion

        #region Constructor

        public ReportisticaViewModel(IRepositoryAttestato repository)
        {
            _repositoryAttestato = repository ?? throw new ArgumentNullException(nameof(repository));

            BDAttestati = new ObservableCollection<Attestati>();
            FilteredBDAttestati = new ObservableCollection<Attestati>();
            _attestatiModificati = new HashSet<int>();

            CaricaDati();
        }

        #endregion

        #region Methods

        private void CaricaDati()
        {
            try
            {
                var attestati = _repositoryAttestato.GetAll();

                // Rimuovi gli handler precedenti
                foreach (var attestato in BDAttestati)
                {
                    attestato.PropertyChanged -= Attestato_PropertyChanged;
                }

                BDAttestati.Clear();
                _attestatiModificati.Clear();

                foreach (var attestato in attestati)
                {
                    // ✅ Sottoscrivi all'evento PropertyChanged
                    attestato.PropertyChanged += Attestato_PropertyChanged;
                    BDAttestati.Add(attestato);
                }

                ApplicaFiltro();

                // Aggiorna lo stato del comando Salva
                CommandManager.InvalidateRequerySuggested();
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

        // ✅ Traccia solo gli attestati modificati
        private void Attestato_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Attestati attestatoModificato && e.PropertyName == nameof(Attestati.Attivo))
            {
                // Aggiungi l'ID alla lista dei modificati
                if (!_attestatiModificati.Contains(attestatoModificato.Id))
                {
                    _attestatiModificati.Add(attestatoModificato.Id);

                    // Aggiorna lo stato del comando Salva
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        // ✅ Salva solo gli attestati modificati
        private void SalvaModifiche()
        {
            try
            {
                if (_attestatiModificati.Count == 0)
                {
                    MessageBox.Show(
                        "Nessuna modifica da salvare.",
                        "Informazione",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    return;
                }

                int contatoreAggiornamenti = 0;

                // Salva solo gli attestati che sono stati modificati
                foreach (var attestato in BDAttestati.Where(a => _attestatiModificati.Contains(a.Id)))
                {
                    _repositoryAttestato.Update(attestato);
                    contatoreAggiornamenti++;
                }

                if(contatoreAggiornamenti==1)
                MessageBox.Show(
                    $"Salvataggio corretto!",
                    "Salvataggio Completato CheckBox",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                else
                    MessageBox.Show(
                     $"Salvataggio di {contatoreAggiornamenti} modifiche",
                     "Salvataggio Completato CheckBox",
                     MessageBoxButton.OK,
                     MessageBoxImage.Information
                 );



                _attestatiModificati.Clear();

                // Aggiorna lo stato del comando Salva (disabilita il pulsante)
                CommandManager.InvalidateRequerySuggested();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Errore durante il salvataggio: {ex.Message}",
                    "Errore",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void ResetFiltro()
        {
            FiltroRicerca = string.Empty;
            _filtroAttivoCorrente = FiltroAttivoEnum.Tutti;
            ApplicaFiltro();
        }

        // ✅ Imposta il filtro per Attivo
        private void ImpostaFiltroAttivo(FiltroAttivoEnum filtro)
        {
            _filtroAttivoCorrente = filtro;
            ApplicaFiltro();
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
                        writer.WriteLine("Matricola;Titolo Corso;Codice Attività;Codice Materia;Ente Formatore;Ente Certificatore;Data Inizio;Data Fine;Anno Corso;Validità Anni;Data Scadenza;Link Attestato;Attivo");

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
                                        $"{EscapaCsv(attestato.LinkAttestato)};" +
                                        $"{(attestato.Attivo ? "Sì" : "No")}";

                            writer.WriteLine(linea);
                        }
                    }

                    MessageBox.Show($"Esportazione completata con successo!\nFile salvato in: {saveFileDialog.FileName}",
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

        // ✅ Filtro combinato: ricerca testuale + stato Attivo
        private void ApplicaFiltro()
        {
            FilteredBDAttestati.Clear();

            // Prima applica il filtro testuale
            IEnumerable<Attestati> risultati = BDAttestati;

            if (!string.IsNullOrWhiteSpace(FiltroRicerca))
            {
                var filtro = FiltroRicerca.ToLower();

                risultati = risultati.Where(a =>
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
            }

            // Poi applica il filtro Attivo
            switch (_filtroAttivoCorrente)
            {
                case FiltroAttivoEnum.SoloAttivi:
                    risultati = risultati.Where(a => a.Attivo);
                    break;
                case FiltroAttivoEnum.SoloNonAttivi:
                    risultati = risultati.Where(a => !a.Attivo);
                    break;
                case FiltroAttivoEnum.Tutti:
                default:
                    // Nessun filtro aggiuntivo
                    break;
            }

            // Aggiungi i risultati alla collection filtrata
            foreach (var attestato in risultati)
            {
                FilteredBDAttestati.Add(attestato);
            }
        }

        public override void Cleanup()
        {
            if (BDAttestati != null)
            {
                foreach (var attestato in BDAttestati)
                {
                    attestato.PropertyChanged -= Attestato_PropertyChanged;
                }
            }
            base.Cleanup();
        }

        #endregion

        #region Enums

        private enum FiltroAttivoEnum
        {
            Tutti,
            SoloAttivi,
            SoloNonAttivi
        }

        #endregion
    }
}