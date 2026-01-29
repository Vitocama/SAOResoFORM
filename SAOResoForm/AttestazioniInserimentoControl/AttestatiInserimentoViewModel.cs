using SAOResoForm.Models;
using SAOResoForm.Service.App;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl.AttestazioniInserimentoControl
{
    public class AttestatiInserimentoViewModel : INotifyPropertyChanged, IDisposable
    {


        private readonly MainViewModel _mainVM;
        private readonly AppServices _appServices;
    


        // ========================
        // COLLEZIONI
        // ========================
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
        // CONSTRUCTOR
        // ========================
        public AttestatiInserimentoViewModel(MainViewModel mainVM, AppServices appServices)
        {
            _appServices = appServices;
            _mainVM = mainVM;

            CaricaDati();
        }

        // ========================
        // CARICAMENTO DATI
        // ========================
        private void CaricaDati()
        {
            var personaleList = _appServices.RepositoryService.GetAll();
            PersonaleList = new ObservableCollection<Personale>(personaleList);
            FilteredPersonaleList = new ObservableCollection<Personale>(personaleList);
        }

        // ========================
        // FILTRO
        // ========================
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
        public void AggiornaDati()
        {
            CaricaDati();
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
}