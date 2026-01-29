using RESOFORM.Dati;
using SAOResoForm.Models;
using SAOResoForm.Service.App;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.AttestratiCreaControl
{
    public class AttestatiCreaViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly AppServices _appServices;
        private readonly Personale _personaleSelezionato;
        private readonly Action _onSalvatoCallback;

        // ========================
        // PROPERTIES - FORM
        // ========================
      

        private string _materia;
        public string Materia
        {
            get => _materia;
            set
            {
                _materia = value;
                OnPropertyChanged();
            }
        }

        private string _enteFormatore;
        public string EnteFormatore
        {
            get => _enteFormatore;
            set
            {
                _enteFormatore = value;
                OnPropertyChanged();
            }
        }

        private string _enteCertificatore;
        public string EnteCertificatore
        {
            get => _enteCertificatore;
            set
            {
                _enteCertificatore = value;
                OnPropertyChanged();
            }
        }

        private string _titoloCorso;
        public string TitoloCorso
        {
            get => _titoloCorso;
            set
            {
                _titoloCorso = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dataInizioCorso;
        public DateTime? DataInizioCorso
        {
            get => _dataInizioCorso;
            set
            {
                _dataInizioCorso = value;
                OnPropertyChanged();
                CalcolaDataScadenza();
            }
        }

        private DateTime? _dataFineCorso;
        public DateTime? DataFineCorso
        {
            get => _dataFineCorso;
            set
            {
                _dataFineCorso = value;
                OnPropertyChanged();
                CalcolaDataScadenza();
            }
        }

        private string _validitaAnni;
        public string ValiditaAnni
        {
            get => _validitaAnni;
            set
            {
                _validitaAnni = value;
                OnPropertyChanged();
                CalcolaDataScadenza();
            }
        }

        private DateTime? _dataScadenza;
        public DateTime? DataScadenza
        {
            get => _dataScadenza;
            set
            {
                _dataScadenza = value;
                OnPropertyChanged();
            }
        }

        private string _note;
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }

        // ========================
        // INFO PERSONALE
        // ========================
        public string PersonaleInfo
        {
            get
            {
                if (_personaleSelezionato != null)
                {
                    return $"{_personaleSelezionato.GradoQualifica} {_personaleSelezionato.Cognome} {_personaleSelezionato.Nome}";
                }
                return string.Empty;
            }
        }

        // ========================
        // VALIDAZIONE
        // ========================
        private string _messaggioValidazione;
        public string MessaggioValidazione
        {
            get => _messaggioValidazione;
            set
            {
                _messaggioValidazione = value;
                OnPropertyChanged();
            }
        }

        // ========================
        // COMMANDS
        // ========================
        public ICommand SalvaCommand { get; }

        // ========================
        // CONSTRUCTOR
        // ========================
        public AttestatiCreaViewModel(Personale personale, AppServices appServices, Action onSalvatoCallback = null)
        {
            _personaleSelezionato = personale ?? throw new ArgumentNullException(nameof(personale));
            _appServices = appServices ?? throw new ArgumentNullException(nameof(appServices));
            _onSalvatoCallback = onSalvatoCallback;

            // Inizializza comando
            SalvaCommand = new RelayCommand(Salva, CanSalva);

            // Carica dati
            CaricaAttivitaFormative();
        }

        // ========================
        // CARICAMENTO DATI
        // ========================
        private void CaricaAttivitaFormative()
        {
          
        }

        // ========================
        // CALCOLO DATA SCADENZA
        // ========================
        private void CalcolaDataScadenza()
        {
            if (DataFineCorso.HasValue && !string.IsNullOrWhiteSpace(ValiditaAnni))
            {
                if (int.TryParse(ValiditaAnni, out int anni) && anni > 0)
                {
                    DataScadenza = DataFineCorso.Value.AddYears(anni);
                }
                else
                {
                    DataScadenza = null;
                }
            }
            else
            {
                DataScadenza = null;
            }
        }

        // ========================
        // VALIDAZIONE
        // ========================
        private bool CanSalva()
        {
            return true; // Sempre abilitato, validazione nel metodo Salva
        }

        private bool ValidaDati()
        {
            // Attività Formativa obbligatoria
          

            // Ente Formatore obbligatorio
            if (string.IsNullOrWhiteSpace(EnteFormatore))
            {
                MessaggioValidazione = "Inserire l'ente formatore";
                return false;
            }

            // Titolo Corso obbligatorio
            if (string.IsNullOrWhiteSpace(TitoloCorso))
            {
                MessaggioValidazione = "Inserire il titolo del corso";
                return false;
            }

            // Data Inizio obbligatoria
            if (!DataInizioCorso.HasValue)
            {
                MessaggioValidazione = "Inserire la data di inizio corso";
                return false;
            }

            // Data Fine obbligatoria
            if (!DataFineCorso.HasValue)
            {
                MessaggioValidazione = "Inserire la data di fine corso";
                return false;
            }

            // Validazione date
            if (DataFineCorso < DataInizioCorso)
            {
                MessaggioValidazione = "La data di fine deve essere successiva alla data di inizio";
                return false;
            }

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
                // Validazione
                if (!ValidaDati())
                {
                    return;
                }

                // Crea nuovo attestato
              

              
                // Salva nel database
              

                // Notifica successo
                MessageBox.Show(
                    $"Attestato salvato con successo per:\n{PersonaleInfo}",
                    "Successo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Callback
                _onSalvatoCallback?.Invoke();

                // Chiudi finestra
                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.DataContext == this)
                    ?.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore salvataggio attestato: {ex.Message}");
                MessageBox.Show(
                    $"Errore durante il salvataggio dell'attestato:\n{ex.Message}",
                    "Errore",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        // ========================
        // DISPOSE
        // ========================
        public void Dispose()
        {
            // Cleanup se necessario
        }

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