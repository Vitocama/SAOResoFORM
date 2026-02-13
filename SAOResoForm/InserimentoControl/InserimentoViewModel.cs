using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.Dati;
using SAOResoForm.Models;
using SAOResoForm.PersonaleControl;
using SAOResoForm.ResourcesDictionary.Dati;
using SAOResoForm.Service.App;
using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.InserimentoControl
{
    public class InserimentoViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainVM;
        private readonly AppServices _appServices;

        // Flag per tracciare se il caricamento è automatico
        private bool _isLoadingData = false;

        public ICommand SalvaCommand { get; }
        public ICommand AnnullaCommand { get; }

        // ------------------- COMBOBOX INCARICO, SERVIZIO, MILITARI -------------------
        public ObservableCollection<string> Incarichi { get; }
        public ObservableCollection<string> Servizio { get; }
        public ObservableCollection<string> Militari { get; }

        private string _incaricoSelezionato;
        public string IncaricoSelezionato
        {
            get => _incaricoSelezionato;
            set
            {
                if (_incaricoSelezionato != value)
                {
                    _incaricoSelezionato = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _servizioSelezionato;
        public string ServizioSelezionato
        {
            get => _servizioSelezionato;
            set
            {
                if (_servizioSelezionato != value)
                {
                    _servizioSelezionato = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _militariSelezionato;
        public string MilitariSelezionato
        {
            get => _militariSelezionato;
            set
            {
                if (_militariSelezionato != value)
                {
                    _militariSelezionato = value;
                    OnPropertyChanged();
                }
            }
        }

        // ------------------- LISTBOX A TRE LIVELLI (REPARTI/SEZIONI/NUCLEI) -------------------
        public ObservableCollection<string> Reparti { get; set; }
        public ObservableCollection<string> Sezioni { get; set; }
        public ObservableCollection<string> Nuclei { get; set; }

        private string _repartoSelezionato;
        public string RepartoSelezionato
        {
            get => _repartoSelezionato;
            set
            {
                if (_repartoSelezionato != value)
                {
                    _repartoSelezionato = value;
                    OnPropertyChanged();
                    CaricaSezioni();
                }
            }
        }

        private string _sezioneSelezionata;
        public string SezioneSelezionata
        {
            get => _sezioneSelezionata;
            set
            {
                if (_sezioneSelezionata != value)
                {
                    _sezioneSelezionata = value;
                    OnPropertyChanged();

                    // Carica nuclei solo se NON è un caricamento automatico
                    if (!_isLoadingData)
                    {
                        CaricaNuclei();
                    }
                }
            }
        }

        private string _nucleoSelezionato;
        public string NucleoSelezionato
        {
            get => _nucleoSelezionato;
            set
            {
                if (_nucleoSelezionato != value)
                {
                    _nucleoSelezionato = value;
                    OnPropertyChanged();
                }
            }
        }

        private Dictionary<string, List<string>> _repartiSezioni;
        private Dictionary<string, List<string>> _sezioniNuclei;

        public InserimentoViewModel(MainViewModel mainVM, AppServices appServices)
        {
            _appServices = appServices;
            _mainVM = mainVM;

            SalvaCommand = new RelayCommand(Salva);
            AnnullaCommand = new RelayCommand(Annulla);

            // Inizializza le collections
            Reparti = new ObservableCollection<string>();
            Sezioni = new ObservableCollection<string>();
            Nuclei = new ObservableCollection<string>();

            // Carica i dizionari
            _repartiSezioni = RepartiSezioniNucleoKeyValue.Data2;
            _sezioniNuclei = RepartiSezioniNucleoKeyValue.TerzoLivello;

            // Carica i reparti (primo livello)
            CaricaReparti();

            // Popola le ComboBox
            DatiComboxInsert incarico = new DatiComboxInsert();
            Incarichi = new ObservableCollection<string>(incarico.incarico);
            Servizio = new ObservableCollection<string>(incarico.stato_di_servizio);
            Militari = new ObservableCollection<string>(incarico.milciv);
        }

        // =====================
        // PROPRIETÀ BINDATE
        // =====================
        private string _nome;
        public string Nome { get => _nome; set { _nome = value; OnPropertyChanged(); } }

        private string _cognome;
        public string Cognome { get => _cognome; set { _cognome = value; OnPropertyChanged(); } }

        private string _matricola;
        public string Matricola { get => _matricola; set { _matricola = value; OnPropertyChanged(); } }

        private string _annotazioni;
        public string Annotazioni { get => _annotazioni; set { _annotazioni = value; OnPropertyChanged(); } }

        private string _categoriaProfilo;
        public string CategoriaProfilo { get => _categoriaProfilo; set { _categoriaProfilo = value; OnPropertyChanged(); } }

        private string _gradoQualifica;
        public string GradoQualifica { get => _gradoQualifica; set { _gradoQualifica = value; OnPropertyChanged(); } }

        // =====================
        // CARICAMENTO LISTBOX A CASCATA
        // =====================
        private void CaricaReparti()
        {
            _isLoadingData = true;

            Reparti.Clear();

            if (_repartiSezioni != null)
            {
                foreach (var reparto in _repartiSezioni.Keys)
                {
                    Reparti.Add(reparto);
                }

                if (Reparti.Count > 0)
                {
                    RepartoSelezionato = Reparti[0];
                }
            }

            _isLoadingData = false;
        }

        private void CaricaSezioni()
        {
            _isLoadingData = true;

            Sezioni.Clear();
            Nuclei.Clear();
            NucleoSelezionato = null;

            if (string.IsNullOrWhiteSpace(RepartoSelezionato))
            {
                SezioneSelezionata = null;
                _isLoadingData = false;
                return;
            }

            if (_repartiSezioni != null && _repartiSezioni.ContainsKey(RepartoSelezionato))
            {
                var sezioniReparto = _repartiSezioni[RepartoSelezionato];

                foreach (var sezione in sezioniReparto)
                {
                    Sezioni.Add(sezione);
                }

                if (Sezioni.Count > 0)
                {
                    SezioneSelezionata = Sezioni[0];
                }
                else
                {
                    SezioneSelezionata = null;
                }
            }

            _isLoadingData = false;
        }

        private void CaricaNuclei()
        {
            Nuclei.Clear();

            if (string.IsNullOrWhiteSpace(SezioneSelezionata) || SezioneSelezionata == " ")
            {
                NucleoSelezionato = null;
                return;
            }

            if (_sezioniNuclei != null && _sezioniNuclei.ContainsKey(SezioneSelezionata))
            {
                var nucleiSezione = _sezioniNuclei[SezioneSelezionata];

                if (nucleiSezione != null && nucleiSezione.Count > 0)
                {
                    foreach (var nucleo in nucleiSezione)
                    {
                        if (!string.IsNullOrWhiteSpace(nucleo))
                        {
                            Nuclei.Add(nucleo);
                        }
                    }

                    if (Nuclei.Count > 0)
                    {
                        NucleoSelezionato = Nuclei[0];
                    }
                    else
                    {
                        NucleoSelezionato = null;
                    }
                }
                else
                {
                    NucleoSelezionato = null;
                }
            }
            else
            {
                NucleoSelezionato = null;
            }
        }

        // =====================
        // SALVA
        // =====================
        private void Salva()
        {
            // Validazioni
            if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Cognome))
            {
                MessageBox.Show("Nome e Cognome sono obbligatori!", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Matricola))
            {
                MessageBox.Show("La Matricola è obbligatoria!", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(GradoQualifica))
            {
                MessageBox.Show("Grado/Qualifica è obbligatorio!", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CategoriaProfilo))
            {
                MessageBox.Show("Categoria/Profilo è obbligatorio!", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(IncaricoSelezionato))
            {
                MessageBox.Show("Seleziona un Incarico!", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ServizioSelezionato))
            {
                MessageBox.Show("Seleziona uno Stato di Servizio!", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(MilitariSelezionato))
            {
                MessageBox.Show("Seleziona Militare o Civile!", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Calcolo COD_UUOO
            Cod_UUOO reparti = new Cod_UUOO();
            string reparto = RepartoSelezionato ?? "";
            string sezione = SezioneSelezionata?.Replace("-", "") ?? "";
            string nucleo = NucleoSelezionato?.Replace("-", "") ?? "";
            string chiave = $"{reparto} {sezione} {nucleo}".Trim();
            int? cod_UUOO = null;
            if (reparti.reparti.ContainsKey(chiave))
            {
                cod_UUOO = reparti.reparti[chiave];
            }
            else
            {
                MessageBox.Show($"Combinazione non trovata nel sistema:\n{chiave}\n\nContatta l'amministratore.",
                    "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
           

            // Creazione oggetto Personale
            var indice = new tblContext().Personale.Max(p => (int?)p.Id) ?? 0;
            var item = new Personale
            {
                Id = indice + 1,
                Nome = Nome,
                Cognome = Cognome,
                Matricola = Matricola,
                Annotazioni = Annotazioni,
                GradoQualifica = GradoQualifica,
                CategoriaProfilo = CategoriaProfilo,
                CodUfficio = cod_UUOO,
                Incarico = IncaricoSelezionato,
                StatoServizio = ServizioSelezionato,
                MilCiv = MilitariSelezionato,
                CodReparto = RepartoSelezionato,
                CodSezione = SezioneSelezionata,
                CodNucleo = NucleoSelezionato
            };

            // Riepilogo
            string riepilogo = $"Confermi di salvare questo personale?\n\n" +
                               $"Nome: {item.Nome}\n" +
                               $"Cognome: {item.Cognome}\n" +
                               $"Matricola: {item.Matricola}\n" +
                               $"Grado/Qualifica: {item.GradoQualifica}\n" +
                               $"Categoria/Profilo: {item.CategoriaProfilo}\n" +
                               $"Mil/Civ: {item.MilCiv}\n" +
                               $"Reparto: {item.CodReparto ?? "N/D"}\n" +
                               $"Sezione: {item.CodSezione ?? "N/D"}\n" +
                               $"Nucleo: {item.CodNucleo ?? "N/D"}\n" +
                               $"Cod. Ufficio: {item.CodUfficio?.ToString() ?? "N/D"}\n" +
                               $"Incarico: {item.Incarico}\n" +
                               $"Stato Servizio: {item.StatoServizio}\n" +
                               $"Annotazioni: {item.Annotazioni ?? "Nessuna"}";

            var result = MessageBox.Show(riepilogo, "Conferma Salvataggio",
                MessageBoxButton.OKCancel, MessageBoxImage.Question);

            // Salvataggio
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    // CORRETTO: usa i metodi di AppServices
                    string salvataggio = _appServices.RepositoryService.Save(item);
                    _appServices.Tool.CreaCartella(item);

                    MessageBox.Show(salvataggio, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    Annulla();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore durante il salvataggio: {ex.Message}",
                        "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Salvataggio annullato", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // =====================
        // ANNULLA
        // =====================
        private void Annulla()
        {
            Nome = string.Empty;
            Cognome = string.Empty;
            Matricola = string.Empty;
            Annotazioni = string.Empty;
            CategoriaProfilo = string.Empty;
            GradoQualifica = string.Empty;
            IncaricoSelezionato = null;
            ServizioSelezionato = null;
            MilitariSelezionato = null;
            CaricaReparti();
        }

        // =====================
        // INotifyPropertyChanged
        // =====================
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}