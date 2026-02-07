using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.Models;
using SAOResoForm.Service.App;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl.AttestazioniInserimentoControl
{
    public class AttestatiInserimentoViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly MainViewModel _mainVM;
        private readonly AppServices _appServices;

        public AppServices AppServices => _appServices;

        // ========================
        // EVENTI
        // ========================
        // Questo evento segnala alla View di aprire AttestatiCreaView
        public event EventHandler<Personale> RichiediAperturaCreaAttestato;

        // ========================
        // COMANDI
        // ========================
        public ICommand ApriCreaAttestatoCommand { get; }

        // ========================
        // COLLEZIONI
        // ========================
        private ObservableCollection<Personale> _personaleList;
        public ObservableCollection<Personale> PersonaleList
        {
            get => _personaleList;
            set { _personaleList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Personale> _filteredPersonaleList;
        public ObservableCollection<Personale> FilteredPersonaleList
        {
            get => _filteredPersonaleList;
            set { _filteredPersonaleList = value; OnPropertyChanged(); }
        }

        // ========================
        // SELEZIONE
        // ========================
        private Personale _personaleSelezionato;
        public Personale PersonaleSelezionato
        {
            get => _personaleSelezionato;
            set
            {
                _personaleSelezionato = value;
                OnPropertyChanged();
                (ApriCreaAttestatoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        // ========================
        // FILTRO RICERCA
        // ========================
        private string _filtroRicerca;
        public string FiltroRicerca
        {
            get => _filtroRicerca;
            set
            {
                _filtroRicerca = value;
                OnPropertyChanged();
                ApplicaFiltro();
            }
        }

        // ========================
        // COSTRUTTORE
        // ========================
        public AttestatiInserimentoViewModel(MainViewModel mainVM, AppServices appServices)
        {
            _mainVM = mainVM;
            _appServices = appServices;

            ApriCreaAttestatoCommand = new RelayCommand(
                ApriCreaAttestato,
                () => PersonaleSelezionato != null);

            CaricaDati();
        }

        // ========================
        // LOGICA
        // ========================
        private void ApriCreaAttestato()
        {
            // Al doppio click, invia alla View l'oggetto selezionato
            if (PersonaleSelezionato != null)
                RichiediAperturaCreaAttestato?.Invoke(this, PersonaleSelezionato);
        }

        private void CaricaDati()
        {
            var personaleList = _appServices.RepositoryService.GetAll();
            PersonaleList = new ObservableCollection<Personale>(personaleList);
            FilteredPersonaleList = new ObservableCollection<Personale>(personaleList);
        }

        private void ApplicaFiltro()
        {
            if (string.IsNullOrWhiteSpace(FiltroRicerca))
            {
                FilteredPersonaleList = new ObservableCollection<Personale>(PersonaleList);
            }
            else
            {
                var filtro = FiltroRicerca.ToLower();
                var risultati = PersonaleList.Where(p =>
                    (p.Cognome?.ToLower().Contains(filtro) ?? false) ||
                    (p.Nome?.ToLower().Contains(filtro) ?? false) ||
                    (p.Matricola?.ToLower().Contains(filtro) ?? false) ||
                    (p.GradoQualifica?.ToLower().Contains(filtro) ?? false) ||
                    (p.CodUfficio?.ToString().Contains(filtro) ?? false)
                ).ToList();

                FilteredPersonaleList = new ObservableCollection<Personale>(risultati);
            }
        }

        // ========================
        // AGGIORNA DATI
        // ========================
        public void AggiornaDati() => CaricaDati();

        // ========================
        // DISPOSE
        // ========================
        public void Dispose()
        {
            // Nessuna risorsa da rilasciare
        }

        // ========================
        // INotifyPropertyChanged
        // ========================
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
