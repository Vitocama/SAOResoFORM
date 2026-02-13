using RESOFORM.Dati;
using SAOResoForm.Models;
using SAOResoForm.Repositories;
using SAOResoForm.Service.Repository.tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl.AttestatiCreaControl
{
    public class AttestatiCreaViewModel : INotifyPropertyChanged
    {
        private readonly IRepositoryAttestato _repositoryAttestato;
        private readonly ITool _tool;

        public event PropertyChangedEventHandler PropertyChanged;

        // Flag per controllare se è già stato salvato
        private bool _isSalvato = false;

        #region proprieta
        // =======================
        // PROPRIETÀ DEL FORM
        // =======================
        public ObservableCollection<Personale> PersonaleSelezionato { get; set; }
        public ObservableCollection<string> AttivitaFormativeList { get; set; } = new ObservableCollection<string>();

        private string _attivitaFormativaSelezionata;
        public string AttivitaFormativaSelezionata
        {
            get => _attivitaFormativaSelezionata;
            set { _attivitaFormativaSelezionata = value; OnPropertyChanged(nameof(AttivitaFormativaSelezionata)); }
        }

        private string _titoloCorso;
        public string TitoloCorso
        {
            get => _titoloCorso;
            set { _titoloCorso = value; OnPropertyChanged(nameof(TitoloCorso)); }
        }

        private string _enteFormatore;
        public string EnteFormatore
        {
            get => _enteFormatore;
            set { _enteFormatore = value; OnPropertyChanged(nameof(EnteFormatore)); }
        }

        private string _enteCertificatore;
        public string EnteCertificatore
        {
            get => _enteCertificatore;
            set { _enteCertificatore = value; OnPropertyChanged(nameof(EnteCertificatore)); }
        }

        private string _materia;
        public string Materia
        {
            get => _materia;
            set { _materia = value; OnPropertyChanged(nameof(Materia)); }
        }

        private DateTime? _dataInizio;
        public DateTime? DataInizio
        {
            get => _dataInizio;
            set { _dataInizio = value; OnPropertyChanged(nameof(DataInizio)); }
        }

        private DateTime? _dataFine;
        public DateTime? DataFine
        {
            get => _dataFine;
            set { _dataFine = value; OnPropertyChanged(nameof(DataFine)); }
        }

        private string _validitaAnni;
        public string ValiditaAnni
        {
            get => _validitaAnni;
            set { _validitaAnni = value; OnPropertyChanged(nameof(ValiditaAnni)); }
        }

        private string _nomeFile;
        public string NomeFile
        {
            get => _nomeFile;
            set { _nomeFile = value; OnPropertyChanged(nameof(NomeFile)); }
        }

        private string _percorsoFile;
        public string PercorsoFile
        {
            get => _percorsoFile;
            set { _percorsoFile = value; OnPropertyChanged(nameof(PercorsoFile)); }
        }

        private string _messaggioValidazione;
        public string MessaggioValidazione
        {
            get => _messaggioValidazione;
            set { _messaggioValidazione = value; OnPropertyChanged(nameof(MessaggioValidazione)); }
        }
        #endregion

        public string PersonaleInfo => PersonaleSelezionato != null && PersonaleSelezionato.Count > 0
            ? string.Join(", ", PersonaleSelezionato.Select(p => $"{p.Nome} {p.Cognome}"))
            : "Nessun personale selezionato";

        // =======================
        // COMANDI
        // =======================
        public ICommand SalvaCommand { get; set; }
        public ICommand CaricaFileCommand { get; set; }
        public ICommand RimuoviFileCommand { get; set; }
        public ICommand AnnullaCommand { get; set; }

        // =======================
        // COSTRUTTORE
        // =======================
        public AttestatiCreaViewModel(List<long> personaleIds, IRepositoryAttestato repositoryAttestato, ITool tool)
        {
            _repositoryAttestato = repositoryAttestato ?? throw new ArgumentNullException(nameof(repositoryAttestato));
            _tool = tool ?? throw new ArgumentNullException(nameof(tool));

            PersonaleSelezionato = new ObservableCollection<Personale>(GetPersonaleByIds(personaleIds));

            // Inizializza comandi con CanExecute per il comando Salva
            SalvaCommand = new RelayCommand(
                async _ => await SalvaAttestatiAsync(),
                _ => !_isSalvato  // Disabilita se già salvato
            );
            CaricaFileCommand = new RelayCommand(_ => CaricaFile());
            RimuoviFileCommand = new RelayCommand(_ => RimuoviFile());
            AnnullaCommand = new RelayCommand(param => { if (param is Window w) w.Close(); });

            // Popola attività formative
            AttivitaFormativaComboBox combo = new AttivitaFormativaComboBox();
            AttivitaFormativeList = new ObservableCollection<string>(combo.formativa);
        }

        // =======================
        // METODI PRIVATI
        // =======================
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(PersonaleSelezionato))
                OnPropertyChanged(nameof(PersonaleInfo));
        }

        private List<Personale> GetPersonaleByIds(List<long> ids)
        {
            var context = new tblContext();
            return context.Personale.Where(p => ids.Contains(p.Id)).ToList();
        }

        private async Task SalvaAttestatiAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(PercorsoFile) || !File.Exists(PercorsoFile))
                {
                    MessaggioValidazione = "Selezionare un file valido.";
                    return;
                }

                // Valida ValiditaAnni
                if (string.IsNullOrWhiteSpace(ValiditaAnni))
                {
                    MessaggioValidazione = "Il campo Validità Anni è obbligatorio.";
                    return;
                }

                if (!int.TryParse(ValiditaAnni, out int validitaAnniInt))
                {
                    MessaggioValidazione = "Validità anni deve essere un numero intero.";
                    return;
                }

                if (validitaAnniInt < 0)
                {
                    MessaggioValidazione = "Validità anni non può essere negativo.";
                    return;
                }

                string cartellaBaseSAO = @"C:\SAO";
                Directory.CreateDirectory(cartellaBaseSAO);

                var db = new tblContext();
                int nuovoId = db.Attestati.Any() ? db.Attestati.Max(a => a.Id) + 1 : 1;

                foreach (var personale in PersonaleSelezionato)
                {
                    string nomeCartellaPersonale = $"{personale.Cognome}_{personale.Nome}_{personale.Matricola}";
                    string percorsoCartellaPersonale = Path.Combine(cartellaBaseSAO, nomeCartellaPersonale);
                    Directory.CreateDirectory(percorsoCartellaPersonale);

                    string percorsoNuovoFile = _tool.RinominaFile(
                        personale,
                        DataFine ?? DateTime.Now,
                        PercorsoFile,
                        percorsoCartellaPersonale
                    );

                    var attestato = new Attestati
                    {
                        Id = nuovoId+1,
                        Dipendente=$"{personale.Cognome} {personale.Nome}" ,
                        MatricolaDipendente = personale.Matricola,
                        CodiceAttivitaFormativa = AttivitaFormativaSelezionata,
                        CodiceMateriaCorso = Materia,
                        EnteFormatore = EnteFormatore,
                        DenominazioneEnteCertificatore = EnteCertificatore,
                        DataFineCorso = DataFine?.ToString("dd-MM-yyyy"),
                        DataInizioCorso = DataInizio?.ToString("dd-MM-yyyy"),
                        ValiditaAnni = validitaAnniInt.ToString(),
                        TitoloCorso = TitoloCorso,
                        DataScadenzaCorso = DataFine.HasValue && validitaAnniInt > 0
        ? DataFine.Value.AddYears(validitaAnniInt).ToString("dd-MM-yyyy")
        : validitaAnniInt == 0
            ? "01-01-3000"
            : null,
                        AnnoCorso = DataFine?.Year.ToString(), 
                        LinkAttestato = percorsoNuovoFile
                    };

                    db.Attestati.Add(attestato);
                }

                await db.SaveChangesAsync();

                // Imposta il flag e disabilita il comando
                _isSalvato = true;
                (SalvaCommand as RelayCommand)?.RaiseCanExecuteChanged();

                MessaggioValidazione = $"Attestati salvati con successo! Totale: {PersonaleSelezionato.Count}";
            }
            catch (Exception ex)
            {
                MessaggioValidazione = $"Errore durante il salvataggio: {ex.Message}";
            }
        }

        private void CaricaFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                Title = "Seleziona un file"
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                PercorsoFile = openFileDialog.FileName;
                NomeFile = Path.GetFileName(PercorsoFile);
            }
        }

        private void RimuoviFile()
        {
            NomeFile = string.Empty;
            PercorsoFile = string.Empty;
        }
    }

    // =======================
    // VALIDATION RULE
    // =======================
    public class NumericValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return new ValidationResult(false, "Campo obbligatorio");

            if (!int.TryParse(value.ToString(), out int numero))
                return new ValidationResult(false, "Inserire solo numeri interi");

            if (numero < 0)
                return new ValidationResult(false, "Non sono ammessi numeri negativi");

            return ValidationResult.ValidResult;
        }
    }

    // =======================
    // RELAYCOMMAND
    // =======================
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}