using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.Models;
using SAOResoForm.Service.App;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
<<<<<<< HEAD
using System.Windows.Input;
=======
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887

namespace SAOResoForm.AttestatiControl.AttestazioniInserimentoControl
{
    public class AttestatiInserimentoViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly MainViewModel _mainVM;
<<<<<<< HEAD
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
=======
        public readonly AppServices _appServices; // Public per accesso dal code-behind
        internal readonly AppServices AppServices;
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
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
        private int _totalePersonale;
        public int TotalePersonale
        {
            get => _totalePersonale;
            set { _totalePersonale = value; OnPropertyChanged(); }
        }

<<<<<<< HEAD
        // ========================
        // COSTRUTTORE
        // ========================
=======
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
        public AttestatiInserimentoViewModel(MainViewModel mainVM, AppServices appServices)
        {
            _mainVM = mainVM;
<<<<<<< HEAD
            _appServices = appServices;

            ApriCreaAttestatoCommand = new RelayCommand(
                ApriCreaAttestato,
                () => PersonaleSelezionato != null);

=======
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
            CaricaDati();
            TotalePersonale = PersonaleList.Count;
        }

<<<<<<< HEAD
        // ========================
        // LOGICA
        // ========================
        private void ApriCreaAttestato()
        {
            // Al doppio click, invia alla View l'oggetto selezionato
            if (PersonaleSelezionato != null)
                RichiediAperturaCreaAttestato?.Invoke(this, PersonaleSelezionato);
        }

=======
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
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
                    (p.Matricola?.ToLower().Contains(filtro) ?? false)
                ).ToList();

                FilteredPersonaleList = new ObservableCollection<Personale>(risultati);
            }
        }

<<<<<<< HEAD
        // ========================
        // AGGIORNA DATI
        // ========================
        public void AggiornaDati() => CaricaDati();
=======
        public void AggiornaDati()
        {
            CaricaDati();
        }
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887

        public void Dispose()
        {
<<<<<<< HEAD
            // Nessuna risorsa da rilasciare
        }

        // ========================
        // INotifyPropertyChanged
        // ========================
=======
            // Cleanup
        }

>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
