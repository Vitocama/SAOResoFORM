using RESOFORM.Dati;
using SAOResoForm.Models;
using SAOResoForm.Service.App;
using SAOResoForm.Service.Repository.tool;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl.AttestatiCreaControl
{
    public class AttestatiCreaViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly AppServices _appServices;
        private readonly Personale _personaleSelezionato;
        private readonly Action _onSalvatoCallback;
        private readonly ITool _tool;

        // ========================
        // PROPERTIES - FORM
        // ========================

        private int _totalePersonale;
        public int TotalePersonale
        {
            get => _totalePersonale;
            set { _totalePersonale = value; OnPropertyChanged(); }
        }


        private string _materia;
        public string Materia
        {
            get => _materia;
            set { _materia = value; OnPropertyChanged(); }
        }

        private string _enteFormatore;
        public string EnteFormatore
        {
            get => _enteFormatore;
            set { _enteFormatore = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _attivitaFormativeList;
        public ObservableCollection<string> AttivitaFormativeList
        {
            get => _attivitaFormativeList;
            set { _attivitaFormativeList = value; OnPropertyChanged(); }
        }

        private string _attivitaFormativaSelezionata;
        public string AttivitaFormativaSelezionata
        {
            get => _attivitaFormativaSelezionata;
            set { _attivitaFormativaSelezionata = value; OnPropertyChanged(); }
        }

        private string _enteCertificatore;
        public string EnteCertificatore
        {
            get => _enteCertificatore;
            set { _enteCertificatore = value; OnPropertyChanged(); }
        }

        private string _titoloCorso;
        public string TitoloCorso
        {
            get => _titoloCorso;
            set { _titoloCorso = value; OnPropertyChanged(); }
        }

        // MODIFICATO: Rinominato da DataInizioCorso a DataInizio
        private DateTime? _dataInizio;
        public DateTime? DataInizio
        {
            get => _dataInizio;
            set
            {
                _dataInizio = value;
                OnPropertyChanged();

                // Aggiorna DisplayDateStart per DataFine
                DisplayDateStartDataFine = _dataInizio;

                // Correggi DataFine se è prima di DataInizio
                if (DataFine.HasValue && DataFine < _dataInizio)
                {
                    DataFine = _dataInizio;
                }

                CalcolaDataScadenza();
            }
        }

        // MODIFICATO: Rinominato da DataFineCorso a DataFine
        private DateTime? _dataFine;
        public DateTime? DataFine
        {
            get => _dataFine;
            set
            {
                _dataFine = value;
                OnPropertyChanged();
                CalcolaDataScadenza();
            }
        }

        // Proprietà per bloccare le date nel DatePicker
        private DateTime? _displayDateStartDataFine;
        public DateTime? DisplayDateStartDataFine
        {
            get => _displayDateStartDataFine;
            set { _displayDateStartDataFine = value; OnPropertyChanged(); }
        }

        private string _validitaAnni;
        public string ValiditaAnni
        {
            get => _validitaAnni;
            set { _validitaAnni = value; OnPropertyChanged(); CalcolaDataScadenza(); }
        }

        private DateTime? _dataScadenza;
        public DateTime? DataScadenza
        {
            get => _dataScadenza;
            set { _dataScadenza = value; OnPropertyChanged(); }
        }

        // ========================
        // FILE PROPERTIES
        // ========================
        private string _nomeFile;
        public string NomeFile
        {
            get => _nomeFile;
            set { _nomeFile = value; OnPropertyChanged(); }
        }

        private string _percorsoFile;
        public string PercorsoFile
        {
            get => _percorsoFile;
            set { _percorsoFile = value; OnPropertyChanged(); }
        }

        // ========================
        // INFO PERSONALE
        // ========================
        public string PersonaleInfo =>
            _personaleSelezionato != null
                ? $"{_personaleSelezionato.Cognome} {_personaleSelezionato.Nome} matr. {_personaleSelezionato.Matricola}"
                : string.Empty;

        // ========================
        // VALIDAZIONE
        // ========================
        private string _messaggioValidazione;
        public string MessaggioValidazione
        {
            get => _messaggioValidazione;
            set { _messaggioValidazione = value; OnPropertyChanged(); }
        }

        // ========================
        // COMMANDS
        // ========================
        public ICommand SalvaCommand { get; }
        public ICommand CaricaFileCommand { get; }
        public ICommand RimuoviFileCommand { get; }

        // ========================
        // CONSTRUCTOR
        // ========================
        public AttestatiCreaViewModel(Personale personale, AppServices appServices, Action onSalvatoCallback = null)
        {
            _personaleSelezionato = personale ?? throw new ArgumentNullException(nameof(personale));
            _appServices = appServices ?? throw new ArgumentNullException(nameof(appServices));
            _onSalvatoCallback = onSalvatoCallback;
            _tool = new Tool(); // AGGIUNTO: Istanzia Tool

            SalvaCommand = new RelayCommand(Salva, CanSalva);
            CaricaFileCommand = new RelayCommand(CaricaFile);
            RimuoviFileCommand = new RelayCommand(RimuoviFile, () => !string.IsNullOrEmpty(PercorsoFile));
           
            CaricaAttivitaFormative();
        }

        private void CaricaAttivitaFormative()
        {
            var attivitaFormativa = new AttivitaFormativaComboBox();
            AttivitaFormativeList = new ObservableCollection<string>(attivitaFormativa.formativa);
        }

        // ========================
        // FILE MANAGEMENT
        // ========================
        private void CaricaFile()
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Title = "Seleziona File Attestato",
                    Filter = "PDF files (*.pdf)|*.pdf|Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
                    FilterIndex = 1
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    PercorsoFile = openFileDialog.FileName;
                    NomeFile = System.IO.Path.GetFileName(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore nel caricamento del file:\n{ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RimuoviFile()
        {
            PercorsoFile = null;
            NomeFile = null;
        }

        // RIMOSSO: Metodo CopiaERinominaFile (ora usa Tool.RinominaFile)

        // ========================
        // CALCOLO DATA SCADENZA
        // ========================
        private void CalcolaDataScadenza()
        {
            if (DataFine.HasValue && int.TryParse(ValiditaAnni, out int anni) && anni > 0)
                DataScadenza = DataFine.Value.AddYears(anni);
            else
                DataScadenza = null;
        }

        // ========================
        // VALIDAZIONE
        // ========================
        private bool CanSalva() => true;

        private bool ValidaDati()
        {
            if (string.IsNullOrWhiteSpace(EnteFormatore)) { MessaggioValidazione = "Inserire l'ente formatore"; return false; }
            if (string.IsNullOrWhiteSpace(TitoloCorso)) { MessaggioValidazione = "Inserire il titolo del corso"; return false; }
            if (!DataInizio.HasValue) { MessaggioValidazione = "Inserire la data di inizio"; return false; }
            if (!DataFine.HasValue) { MessaggioValidazione = "Inserire la data di fine"; return false; }
            if (DataFine < DataInizio) { MessaggioValidazione = "La data di fine deve essere successiva alla data di inizio"; return false; }

            MessaggioValidazione = string.Empty;
            return true;
        }

        // ========================
        // SALVA
        // ========================
        private void Salva()
        {
            try
            {
                if (!ValidaDati()) return;

                string percorsoFileFinale = null;
                string nomeFileFinale = null;

                // MODIFICATO: Usa Tool.RinominaFile con DataFine
                if (!string.IsNullOrEmpty(PercorsoFile) && DataFine.HasValue)
                {
                    percorsoFileFinale = _tool.RinominaFile(_personaleSelezionato, DataFine.Value, PercorsoFile);
                    nomeFileFinale = System.IO.Path.GetFileName(percorsoFileFinale);
                }

                MessageBox.Show(
                    $"Attestato salvato con successo per:\n{PersonaleInfo}" +
                    (percorsoFileFinale != null ? $"\n\nFile salvato in:\n{percorsoFileFinale}" : ""),
                    "Successo", MessageBoxButton.OK, MessageBoxImage.Information
                );

                _onSalvatoCallback?.Invoke();

                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.DataContext == this)
                    ?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante il salvataggio dell'attestato:\n{ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ========================
        // DISPOSE
        // ========================
        public void Dispose() { }

        // ========================
        // PROPERTY CHANGED
        // ========================
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // ========================
    // RELAY COMMAND
    // ========================
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();
    }
}