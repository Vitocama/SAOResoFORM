using SAOResoForm.Models;
using SAOResoForm.Service.App;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SAOResoForm.AttestatiControl.AttestazioniInserimentoControl
{
    public class AttestatiInserimentoViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly MainViewModel _mainVM;
        public readonly AppServices _appServices; // Public per accesso dal code-behind
        internal readonly AppServices AppServices;
        private ObservableCollection<Personale> _personaleList;
        public ObservableCollection<Personale> PersonaleList
        {
            get => _personaleList;
            set
            {
                _personaleList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Personale> _filteredPersonaleList;
        public ObservableCollection<Personale> FilteredPersonaleList
        {
            get => _filteredPersonaleList;
            set
            {
                _filteredPersonaleList = value;
                OnPropertyChanged();
            }
        }

        private Personale _personaleSelezionato;
        public Personale PersonaleSelezionato
        {
            get => _personaleSelezionato;
            set
            {
                _personaleSelezionato = value;
                OnPropertyChanged();
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

        public AttestatiInserimentoViewModel(MainViewModel mainVM, AppServices appServices)
        {
            _appServices = appServices;
            _mainVM = mainVM;
            CaricaDati();
            TotalePersonale = PersonaleList.Count;
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
                    (p.Matricola?.ToLower().Contains(filtro) ?? false)
                ).ToList();

                FilteredPersonaleList = new ObservableCollection<Personale>(risultati);
            }
        }

        public void AggiornaDati()
        {
            CaricaDati();
        }

        public void Dispose()
        {
            // Cleanup
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}