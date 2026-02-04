using SAOResoForm.AttestatiControl.AttestatiCreaControl;
using SAOResoForm.Models;
using SAOResoForm.Service.App;
using SAOResoForm.Service.Repository;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SAOResoForm.informazioneControl
{
    public class InformazioneViewModel : INotifyPropertyChanged
    {
        private readonly AppServices _appServices;
        private ObservableCollection<Personale> _personaleInserito;
        private ObservableCollection<PersonaleSelezionabile> _filteredPersonaleList;
        private string _filtroRicerca;
        private int _totalePersonale;
        private PersonaleSelezionabile _personaleSelezionato;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        public ObservableCollection<Personale> PersonaleInserito
        {
            get => _personaleInserito;
            set
            {
                _personaleInserito = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PersonaleSelezionabile> FilteredPersonaleList
        {
            get => _filteredPersonaleList;
            set
            {
                _filteredPersonaleList = value;
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
                ApplicaFiltro();
            }
        }

        public int TotalePersonale
        {
            get => _totalePersonale;
            set
            {
                _totalePersonale = value;
                OnPropertyChanged();
            }
        }

        public PersonaleSelezionabile PersonaleSelezionato
        {
            get => _personaleSelezionato;
            set
            {
                _personaleSelezionato = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AggiungiPersonaleCommand { get; }
        public ICommand AggiungiSelezionatiCommand { get; }
        public ICommand RimuoviPersonaleCommand { get; }
        public ICommand PulisciSelezioneCommand { get; }
        public ICommand CreaAttestatoCommand { get; }

        #endregion

        public InformazioneViewModel()
        {
            
            PersonaleInserito = new ObservableCollection<Personale>();
            FilteredPersonaleList = new ObservableCollection<PersonaleSelezionabile>();

            // Inizializza i comandi
            AggiungiPersonaleCommand = new RelayCommand(AggiungiPersonale, CanAggiungiPersonale);
            AggiungiSelezionatiCommand = new RelayCommand(AggiungiSelezionati, CanAggiungiSelezionati);
            RimuoviPersonaleCommand = new RelayCommand(RimuoviPersonale, CanRimuoviPersonale);
            PulisciSelezioneCommand = new RelayCommand(PulisciSelezione, CanPulisciSelezione);
            CreaAttestatoCommand = new RelayCommand(CreaAttestato, CanCreaAttestato);

            CaricaPersonale();
        }

        #region Metodi Privati

        private void CaricaPersonale()
        {
            using (var context = new tblContext())
            {
                var personaleList = context.Personale.ToList();
                TotalePersonale = personaleList.Count;

                FilteredPersonaleList.Clear();
                foreach (var p in personaleList)
                {
                    FilteredPersonaleList.Add(new PersonaleSelezionabile { Personale = p });
                }
            }
        }

        private void ApplicaFiltro()
        {
            using (var context = new tblContext())
            {
                var query = context.Personale.AsQueryable();

                if (!string.IsNullOrWhiteSpace(FiltroRicerca))
                {
                    string filtro = FiltroRicerca.ToLower();
                    query = query.Where(p =>
                        p.Cognome.ToLower().Contains(filtro) ||
                        p.Nome.ToLower().Contains(filtro) ||
                        p.Matricola.Contains(filtro));
                }

                var risultati = query.OrderBy(p => p.Cognome)
                                    .ThenBy(p => p.Nome)
                                    .ToList();

                FilteredPersonaleList.Clear();
                foreach (var p in risultati)
                {
                    FilteredPersonaleList.Add(new PersonaleSelezionabile { Personale = p });
                }
            }
        }

        #endregion

        #region Command Methods

        private void AggiungiPersonale(object parameter)
        {
            if (PersonaleSelezionato != null)
            {
                // Verifica se non è già presente (confronto per Id)
                if (!PersonaleInserito.Any(p => p.Id == PersonaleSelezionato.Id))
                {
                    PersonaleInserito.Add(PersonaleSelezionato.Personale);
                }
            }
        }

        private bool CanAggiungiPersonale(object parameter)
        {
            return PersonaleSelezionato != null &&
                   !PersonaleInserito.Any(p => p.Id == PersonaleSelezionato.Id);
        }

        private void AggiungiSelezionati(object parameter)
        {
            // Trova tutti i personale con IsSelected = true
            var selezionati = FilteredPersonaleList.Where(p => p.IsSelected).ToList();

            foreach (var personaleWrapper in selezionati)
            {
                // Verifica se non è già presente nella lista inseriti
                if (!PersonaleInserito.Any(p => p.Id == personaleWrapper.Id))
                {
                    PersonaleInserito.Add(personaleWrapper.Personale);
                }
            }

            // Deseleziona tutti dopo l'aggiunta
            foreach (var personaleWrapper in selezionati)
            {
                personaleWrapper.IsSelected = false;
            }
        }

        private bool CanAggiungiSelezionati(object parameter)
        {
            return FilteredPersonaleList != null &&
                   FilteredPersonaleList.Any(p => p.IsSelected);
        }

        private void RimuoviPersonale(object parameter)
        {
            var personale = parameter as Personale;
            if (personale != null)
            {
                PersonaleInserito.Remove(personale);
            }
        }

        private bool CanRimuoviPersonale(object parameter)
        {
            return parameter is Personale;
        }

        private void PulisciSelezione(object parameter)
        {
            PersonaleInserito.Clear();

            // Deseleziona anche tutti i checkbox nel DataGrid
            foreach (var personale in FilteredPersonaleList)
            {
                personale.IsSelected = false;
            }
        }

        private bool CanPulisciSelezione(object parameter)
        {
            return PersonaleInserito.Count > 0;
        }

        private void CreaAttestato(object parameter)
        {
            // Apre la finestra AttestatiCreaView passando il personale selezionato
            var attestatiView = new AttestatiCreaView();
            attestatiView.ShowDialog();
        }

        private bool CanCreaAttestato(object parameter)
        {
            return PersonaleInserito.Count > 0;
        }

        #endregion

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Classe wrapper per aggiungere la funzionalità di selezione tramite checkbox
    /// alla classe Personale senza modificarla direttamente
    /// </summary>
    public class PersonaleSelezionabile : INotifyPropertyChanged
    {
        private bool _isSelected;

        public Personale Personale { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        // Proprietà delegate per binding diretto
        public long Id => Personale.Id;
        public string Cognome => Personale.Cognome;
        public string Nome => Personale.Nome;
        public string Matricola => Personale.Matricola;
        public string DisplayText => $"{Cognome}_{Nome}_{Matricola}";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}