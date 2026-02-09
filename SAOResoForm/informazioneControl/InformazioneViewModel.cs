using Microsoft.EntityFrameworkCore;
using SAOResoForm.AttestatiControl.AttestatiCreaControl;
using SAOResoForm.Models;
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

namespace SAOResoForm.informazioneControl
{
    public class InformazioneViewModel : INotifyPropertyChanged
    {
        private readonly IRepositoryService _repositoryService;
        private readonly ITool _tool;

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
            set { _personaleInserito = value; OnPropertyChanged(); }
        }

        public ObservableCollection<PersonaleSelezionabile> FilteredPersonaleList
        {
            get => _filteredPersonaleList;
            set { _filteredPersonaleList = value; OnPropertyChanged(); }
        }

        public string FiltroRicerca
        {
            get => _filtroRicerca;
            set { _filtroRicerca = value; OnPropertyChanged(); ApplicaFiltro(); }
        }

        public int TotalePersonale
        {
            get => _totalePersonale;
            set { _totalePersonale = value; OnPropertyChanged(); }
        }

        public PersonaleSelezionabile PersonaleSelezionato
        {
            get => _personaleSelezionato;
            set { _personaleSelezionato = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand AggiungiPersonaleCommand { get; }
        public ICommand AggiungiSelezionatiCommand { get; }
        public ICommand RimuoviPersonaleCommand { get; }
        public ICommand PulisciSelezioneCommand { get; }
        public ICommand CreaAttestatoCommand { get; }
        #endregion

        #region Constructor
        public InformazioneViewModel(IRepositoryService repositoryService, ITool tool)
        {
            _repositoryService = repositoryService;
            _tool = tool;

            PersonaleInserito = new ObservableCollection<Personale>();
            FilteredPersonaleList = new ObservableCollection<PersonaleSelezionabile>();

            AggiungiPersonaleCommand = new RelayCommand(AggiungiPersonale, CanAggiungiPersonale);
            AggiungiSelezionatiCommand = new RelayCommand(AggiungiSelezionati, CanAggiungiSelezionati);
            RimuoviPersonaleCommand = new RelayCommand(RimuoviPersonale, CanRimuoviPersonale);
            PulisciSelezioneCommand = new RelayCommand(PulisciSelezione, CanPulisciSelezione);
            CreaAttestatoCommand = new RelayCommand(CreaAttestato, CanCreaAttestato);

            CaricaPersonale();
        }
        #endregion

        #region Private Methods
        private void CaricaPersonale()
        {
             var context = new tblContext();
            var personaleList = context.Personale.AsNoTracking().ToList();

            TotalePersonale = personaleList.Count;

            FilteredPersonaleList.Clear();
            foreach (var p in personaleList)
            {
                FilteredPersonaleList.Add(new PersonaleSelezionabile { Personale = p });
            }
        }

        private void ApplicaFiltro()
        {
             var context = new tblContext();
            var query = context.Personale.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(FiltroRicerca))
            {
                string filtro = FiltroRicerca.ToLower();
                query = query.Where(p => p.Cognome.ToLower().Contains(filtro)
                                       || p.Nome.ToLower().Contains(filtro)
                                       || p.Matricola.Contains(filtro));
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
        #endregion

        #region Command Methods
        private void AggiungiPersonale(object parameter)
        {
            if (PersonaleSelezionato != null && !PersonaleInserito.Any(p => p.Id == PersonaleSelezionato.Id))
            {
                PersonaleInserito.Add(PersonaleSelezionato.Personale);
            }
        }

        private bool CanAggiungiPersonale(object parameter) => PersonaleSelezionato != null &&
                                                             !PersonaleInserito.Any(p => p.Id == PersonaleSelezionato.Id);

        private void AggiungiSelezionati(object parameter)
        {
            var selezionati = FilteredPersonaleList.Where(p => p.IsSelected).ToList();

            foreach (var p in selezionati)
            {
                if (!PersonaleInserito.Any(x => x.Id == p.Id))
                    PersonaleInserito.Add(p.Personale);

                p.IsSelected = false;
            }
        }

        private bool CanAggiungiSelezionati(object parameter) =>
            FilteredPersonaleList != null && FilteredPersonaleList.Any(p => p.IsSelected);

        private void RimuoviPersonale(object parameter)
        {
            if (parameter is Personale personale)
                PersonaleInserito.Remove(personale);
        }

        private bool CanRimuoviPersonale(object parameter) => parameter is Personale;

        private void PulisciSelezione(object parameter)
        {
            PersonaleInserito.Clear();
            foreach (var p in FilteredPersonaleList)
                p.IsSelected = false;
        }

        private bool CanPulisciSelezione(object parameter) => PersonaleInserito.Count > 0;

        private void CreaAttestato(object parameter)
        {
            if (PersonaleInserito.Count == 0) return;

            // Passa solo gli ID al nuovo View
            var personaleIds = PersonaleInserito.Select(p => p.Id).ToList();
            var view = new AttestatiCreaView(personaleIds);
            view.ShowDialog();
        }

        private bool CanCreaAttestato(object parameter) => PersonaleInserito.Count > 0;
        #endregion

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class PersonaleSelezionabile : INotifyPropertyChanged
    {
        private bool _isSelected;
        public Personale Personale { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public long Id => Personale.Id;
        public string Cognome => Personale.Cognome;
        public string Nome => Personale.Nome;
        public string Matricola => Personale.Matricola;
        public string DisplayText => $"{Cognome}_{Nome}_{Matricola}";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

