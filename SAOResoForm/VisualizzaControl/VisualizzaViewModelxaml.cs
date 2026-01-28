using SAOResoForm.Models;
using SAOResoForm.Service.App;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace SAOResoForm.VisualizzaControl
{
    public class VisualizzaViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly MainViewModel _mainVM;
        private readonly AppServices _services;

        private ObservableCollection<Personale> _personaleList;
        private ObservableCollection<Personale> _filteredPersonaleList;
        private Personale _personaleSelezionato;
        private string _filtroRicerca;

        // Lista completa (non filtrata)
        public ObservableCollection<Personale> PersonaleList
        {
            get => _personaleList;
            set
            {
                _personaleList = value;
                OnPropertyChanged();
            }
        }

        // Lista filtrata visualizzata nel DataGrid
        public ObservableCollection<Personale> FilteredPersonaleList
        {
            get => _filteredPersonaleList;
            set
            {
                _filteredPersonaleList = value;
                OnPropertyChanged();
            }
        }

        public Personale PersonaleSelezionato
        {
            get => _personaleSelezionato;
            set
            {
                _personaleSelezionato = value;
                OnPropertyChanged();
            }
        }

        public string FiltroRicerca
        {
            get => _filtroRicerca;
            set
            {
                _filtroRicerca = value;
                OnPropertyChanged();
                FiltraPersonale();
            }
        }

        public ICommand AggiornaCommand { get; }
        public ICommand ChiudiCommand { get; }

        public VisualizzaViewModel(MainViewModel mainVM, AppServices services)
        {
            _mainVM = mainVM;
            _services = services;

            // ============ INIZIALIZZA LE OBSERVABLECOLLECTION PRIMA ============
            PersonaleList = new ObservableCollection<Personale>();
            FilteredPersonaleList = new ObservableCollection<Personale>();

            AggiornaCommand = new RelayCommand(CaricaPersonale);
            ChiudiCommand = new RelayCommand(Chiudi);

            CaricaPersonale();
        }

        private void OnPersonaleModificato(object sender, EventArgs e)
        {
            CaricaPersonale();
        }

        // ============ METODO PUBBLICO PER RICARICARE I DATI ============
        public void CaricaPersonale()
        {
            try
            {
                var dati = _services.RepositoryService.GetAll();

                // ============ SVUOTA E RICARICA SENZA CREARE NUOVA ISTANZA ============
                PersonaleList.Clear();
                foreach (var persona in dati)
                {
                    PersonaleList.Add(persona);
                }

                // Svuota e ricarica anche FilteredPersonaleList
                FilteredPersonaleList.Clear();

                if (string.IsNullOrWhiteSpace(FiltroRicerca))
                {
                    foreach (var persona in dati)
                    {
                        FilteredPersonaleList.Add(persona);
                    }
                }
                else
                {
                    FiltraPersonale();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Errore nel caricamento dei dati: {ex.Message}",
                    "Errore",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private void FiltraPersonale()
        {
            if (PersonaleList == null)
                return;

            if (string.IsNullOrWhiteSpace(FiltroRicerca))
            {
                FilteredPersonaleList.Clear();
                foreach (var persona in PersonaleList)
                {
                    FilteredPersonaleList.Add(persona);
                }
                return;
            }

            var filtroLower = FiltroRicerca.ToLower();

            var filtrati = PersonaleList.Where(p =>
                (p.Id.ToString().Contains(filtroLower)) ||
                (p.Nome?.ToLower().Contains(filtroLower) ?? false) ||
                (p.Cognome?.ToLower().Contains(filtroLower) ?? false) ||
                (p.Matricola?.ToLower().Contains(filtroLower) ?? false) ||
                (p.GradoQualifica?.ToLower().Contains(filtroLower) ?? false) ||
                (p.CategoriaProfilo?.ToLower().Contains(filtroLower) ?? false) ||
                (p.MilCiv?.ToLower().Contains(filtroLower) ?? false) ||
                (p.CodReparto?.ToLower().Contains(filtroLower) ?? false) ||
                (p.CodSezione?.ToLower().Contains(filtroLower) ?? false) ||
                (p.CodNucleo?.ToLower().Contains(filtroLower) ?? false) ||
                (p.CodUfficio.ToString().Contains(filtroLower)) ||
                (p.Incarico?.ToLower().Contains(filtroLower) ?? false) ||
                (p.StatoServizio?.ToLower().Contains(filtroLower) ?? false) ||
                (p.Annotazioni?.ToLower().Contains(filtroLower) ?? false)
            ).ToList();

            FilteredPersonaleList.Clear();
            foreach (var persona in filtrati)
            {
                FilteredPersonaleList.Add(persona);
            }
        }

        private void Chiudi()
        {
            _mainVM.CurrentViewModel = new SAOResoForm.HomeControl.HomeViewModel();
        }

        public void Dispose()
        {
            // Disiscriviti dall'evento
            // _services.RepositoryService.PersonaleModificato -= OnPersonaleModificato;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}