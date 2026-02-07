using RESOFORM.Dati;
using SAOResoForm.Common;
using SAOResoForm.Models;
using SAOResoForm.Service.App;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl.AttestatiCreaControl
{
    public class AttestatiCreaViewModel : INotifyPropertyChanged
    {
        private readonly AppServices _appServices;
        private readonly Personale _personaleSelezionato;
        private readonly Action _onSalvatoCallback;

        // ========================
        // EVENTI PER LA VIEW
        // ========================
<<<<<<< HEAD
        public event EventHandler RichiediChiusura;
        public event EventHandler<string> MostraMessaggioSuccesso;
        public event EventHandler<string> MostraMessaggioErrore;
        public event EventHandler<bool> RichiediConfermaAnnullamento;

        // ========================
        // PROPERTIES FORM
        // ========================
        private string _attivitaFormativa;
        public string AttivitaFormativa
        {
            get => _attivitaFormativa;
            set { _attivitaFormativa = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> AttivitaFormativeList { get; private set; }

=======
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
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

        private DateTime? _dataInizioCorso;
        public DateTime? DataInizioCorso
        {
            get => _dataInizioCorso;
            set { _dataInizioCorso = value; OnPropertyChanged(); CalcolaDataScadenza(); }
        }

        private DateTime? _dataFineCorso;
        public DateTime? DataFineCorso
        {
            get => _dataFineCorso;
            set { _dataFineCorso = value; OnPropertyChanged(); CalcolaDataScadenza(); }
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
            private set { _dataScadenza = value; OnPropertyChanged(); }
        }

        // ========================
        // FILE PROPERTIES
        // ========================
        private string _nomeFile;
        public string NomeFile
        {
<<<<<<< HEAD
            get => _note;
            set { _note = value; OnPropertyChanged(); }
        }

        // ========================
        // FILE
        // ========================
        private string _percorsoFileOriginale;
        public string PercorsoFileOriginale
        {
            get => _percorsoFileOriginale;
            set
            {
                _percorsoFileOriginale = value;
=======
            get => _nomeFile;
            set
            {
                _nomeFile = value;
                OnPropertyChanged();
            }
        }

        private string _percorsoFile;
        public string PercorsoFile
        {
            get => _percorsoFile;
            set
            {
                _percorsoFile = value;
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
                OnPropertyChanged();
                OnPropertyChanged(nameof(NomeFileSelezionato));
            }
        }

        public string NomeFileSelezionato =>
            string.IsNullOrWhiteSpace(PercorsoFileOriginale)
                ? "Nessun file selezionato"
                : Path.GetFileName(PercorsoFileOriginale);

        // ========================
        // INFO PERSONALE
        // ========================
        public string PersonaleInfo =>
            $"{_personaleSelezionato.GradoQualifica} {_personaleSelezionato.Cognome} {_personaleSelezionato.Nome}";

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
<<<<<<< HEAD
        public ICommand SelezionaFileCommand { get; }
        public ICommand AnnullaCommand { get; }
=======
        public ICommand CaricaFileCommand { get; }
        public ICommand RimuoviFileCommand { get; }
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887

        // ========================
        // CONSTRUCTOR
        // ========================
        public AttestatiCreaViewModel(Personale personale, AppServices appServices, Action onSalvatoCallback = null)
        {
            _personaleSelezionato = personale ?? throw new ArgumentNullException(nameof(personale));
            _appServices = appServices ?? throw new ArgumentNullException(nameof(appServices));
            _onSalvatoCallback = onSalvatoCallback;

<<<<<<< HEAD
            SalvaCommand = new RelayCommand(Salva);
            SelezionaFileCommand = new RelayCommand(SelezionaFile);
            AnnullaCommand = new RelayCommand(Annulla);
=======
            // Inizializza comandi
            SalvaCommand = new RelayCommand(Salva, CanSalva);
            CaricaFileCommand = new RelayCommand(CaricaFile);
            RimuoviFileCommand = new RelayCommand(RimuoviFile, () => !string.IsNullOrEmpty(PercorsoFile));
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887

            CaricaAttivitaFormative();
        }

        // ========================
        // DATI
        // ========================
        private void CaricaAttivitaFormative()
        {
<<<<<<< HEAD
            AttivitaFormativeList = new ObservableCollection<string>
            {
                "Corso di Formazione",
                "Aggiornamento Professionale",
                "Abilitazione/Certificazione",
                "Addestramento Operativo",
                "Corso di Specializzazione",
                "Seminario/Convegno",
                "Workshop",
                "Master",
                "Altro"
            };
=======
            // TODO: Carica attività formative dal database
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
                MessageBox.Show(
                    $"Errore nel caricamento del file:\n{ex.Message}",
                    "Errore",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void RimuoviFile()
        {
            PercorsoFile = null;
            NomeFile = null;
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
        }

        // ========================
        // LOGICA
        // ========================
        private void CalcolaDataScadenza()
        {
            if (DataFineCorso.HasValue &&
                int.TryParse(ValiditaAnni, out int anni) &&
                anni > 0)
            {
                DataScadenza = DataFineCorso.Value.AddYears(anni);
            }
            else
            {
                DataScadenza = null;
            }
        }

        private bool ValidaDati()
        {
<<<<<<< HEAD
            if (string.IsNullOrWhiteSpace(AttivitaFormativa))
                return Errore("Selezionare un'attività formativa");

=======
            // Ente Formatore obbligatorio
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
            if (string.IsNullOrWhiteSpace(EnteFormatore))
                return Errore("Inserire l'ente formatore");

            if (string.IsNullOrWhiteSpace(TitoloCorso))
                return Errore("Inserire il titolo del corso");

            if (!DataInizioCorso.HasValue || !DataFineCorso.HasValue)
                return Errore("Inserire le date del corso");

            if (DataFineCorso < DataInizioCorso)
                return Errore("La data di fine non può essere precedente");

            MessaggioValidazione = string.Empty;
            return true;
        }

        private bool Errore(string messaggio)
        {
            MessaggioValidazione = messaggio;
            return false;
        }

        // ========================
        // COMMAND HANDLERS
        // ========================
        private void Salva()
        {
            try
            {
                if (!ValidaDati())
                    return;

<<<<<<< HEAD
                string fileAllegato = SalvaFile();

                var attestato = new Attestati
                {
                    DenominazioneEnteFormatore = AttivitaFormativa,
                    CodiceMateriaCorso = Materia,
                    EnteFormatore = EnteFormatore,
                    DenominazioneEnteCertificatore = EnteCertificatore,
                    TitoloCorso = TitoloCorso,
                    DataInizioCorso = DataInizioCorso?.ToString("dd/MM/yyyy"),
                    DataFineCorso = DataFineCorso?.ToString("dd/MM/yyyy"),
                    ValiditaAnni = ValiditaAnni,
                    DataScadenzaCorso = DataScadenza?.ToString("dd/MM/yyyy"),
                    LinkAttestato = fileAllegato
                };
=======
                // TODO: Crea nuovo attestato e salva nel database
                /*
                var nuovoAttestato = new Attestato
                {
                    Matricola = _personaleSelezionato.Matricola,
                    Cognome = _personaleSelezionato.Cognome,
                    Nome = _personaleSelezionato.Nome,
                    Materia = Materia,
                    EnteFormatore = EnteFormatore,
                    EnteCertificatore = EnteCertificatore,
                    TitoloCorso = TitoloCorso,
                    DataInizioCorso = DataInizioCorso.Value,
                    DataFineCorso = DataFineCorso.Value,
                    ValiditaAnni = ValiditaAnni,
                    DataScadenza = DataScadenza,
                    NomeFile = NomeFile,
                    PercorsoFile = PercorsoFile
                };

                _appServices.AttestatoRepository.Add(nuovoAttestato);
                _appServices.AttestatoRepository.SaveChanges();
                */
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887

                var ctx = new tblContext();
                ctx.Attestati.Add(attestato);
                ctx.SaveChanges();

                MostraMessaggioSuccesso?.Invoke(this,
                    $"Attestato salvato con successo per:\n{PersonaleInfo}");

                _onSalvatoCallback?.Invoke();
                RichiediChiusura?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MostraMessaggioErrore?.Invoke(this, ex.Message);
            }
        }

        private void Annulla()
        {
            RichiediConfermaAnnullamento?.Invoke(this, true);
        }

        public void ConfermaAnnullamento()
        {
            RichiediChiusura?.Invoke(this, EventArgs.Empty);
        }

        private void SelezionaFile()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Tutti i file (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
                PercorsoFileOriginale = dlg.FileName;
        }

        private string SalvaFile()
        {
            if (string.IsNullOrWhiteSpace(PercorsoFileOriginale))
                return null;

            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Allegati", "Attestati");
            Directory.CreateDirectory(dir);

            var nome = Path.GetFileName(PercorsoFileOriginale);
            var dest = Path.Combine(dir, nome);

            File.Copy(PercorsoFileOriginale, dest, true);
            return nome;
        }

        // ========================
        // INotifyPropertyChanged
        // ========================
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
