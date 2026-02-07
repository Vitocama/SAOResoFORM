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

namespace SAOResoForm.AttestratiCreaControl
{
    public class AttestatiCreaViewModel : INotifyPropertyChanged
    {
        private readonly AppServices _appServices;
        private readonly Personale _personaleSelezionato;
        private readonly Action _onSalvatoCallback;

        // ========================
        // EVENTI PER LA VIEW
        // ========================
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

        private string _note;
        public string Note
        {
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
        public ICommand SelezionaFileCommand { get; }
        public ICommand AnnullaCommand { get; }

        // ========================
        // CONSTRUCTOR
        // ========================
        public AttestatiCreaViewModel(Personale personale, AppServices appServices, Action onSalvatoCallback = null)
        {
            _personaleSelezionato = personale ?? throw new ArgumentNullException(nameof(personale));
            _appServices = appServices ?? throw new ArgumentNullException(nameof(appServices));
            _onSalvatoCallback = onSalvatoCallback;

            SalvaCommand = new RelayCommand(Salva);
            SelezionaFileCommand = new RelayCommand(SelezionaFile);
            AnnullaCommand = new RelayCommand(Annulla);

            CaricaAttivitaFormative();
        }

        // ========================
        // DATI
        // ========================
        private void CaricaAttivitaFormative()
        {
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
            if (string.IsNullOrWhiteSpace(AttivitaFormativa))
                return Errore("Selezionare un'attività formativa");

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
