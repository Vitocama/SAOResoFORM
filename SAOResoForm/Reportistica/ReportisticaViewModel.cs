using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using SAOResoForm.Models;
using SAOResoForm.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.Reportistica
{
    public class ReportisticaViewModel : ViewModelBase
    {
        private readonly ITool _tool;
        #region Fields

        private readonly IRepositoryAttestato _repositoryAttestato;

        private ObservableCollection<Attestati> _bdAttestati;
        private ObservableCollection<Attestati> _filteredBDAttestati;
        private string _filtroRicerca;

        private HashSet<int> _attestatiModificati;
        private FiltroAttivoEnum _filtroAttivoCorrente = FiltroAttivoEnum.Tutti;

        #endregion

        #region Properties

        public ObservableCollection<Attestati> BDAttestati
        {
            get => _bdAttestati;
            set => Set(ref _bdAttestati, value);
        }

        public ObservableCollection<Attestati> FilteredBDAttestati
        {
            get => _filteredBDAttestati;
            set => Set(ref _filteredBDAttestati, value);
        }

        public string FiltroRicerca
        {
            get => _filtroRicerca;
            set => Set(ref _filtroRicerca, value);
        }

        #endregion

        #region Commands

        // ✅ AggiornaCommand è l'UNICO che ricarica dal DB
        public ICommand AggiornaCommand => new RelayCommand(CaricaDati);

        // ✅ Questi lavorano SOLO sulla collection in memoria
        public ICommand ApplicaFiltroCommand => new RelayCommand(ApplicaFiltro);
        public ICommand ResetFiltroCommand => new RelayCommand(ResetFiltro);

        // ✅ CORRETTI: chiamano ImpostaFiltroAttivo, NON il repository
        public ICommand MostraTuttiCommand => new RelayCommand(() => ImpostaFiltroAttivo(FiltroAttivoEnum.Tutti));
        public ICommand MostraAttiviCommand => new RelayCommand(() => ImpostaFiltroAttivo(FiltroAttivoEnum.SoloAttivi));
        public ICommand MostraNonAttiviCommand => new RelayCommand(() => ImpostaFiltroAttivo(FiltroAttivoEnum.SoloNonAttivi));

        public ICommand EsportaCommand => new RelayCommand(EsportaInCsv);
        public ICommand SalvaCommand => new RelayCommand(SalvaModifiche, () => _attestatiModificati?.Count > 0);
        public ICommand ApriDocumentoCommand => new RelayCommand<Attestati>(ApriDocumento);

        #endregion

        #region Constructor

        public ReportisticaViewModel(IRepositoryAttestato repository,ITool tool)
        {
            _tool = tool ?? throw new ArgumentNullException(nameof(tool));
            _repositoryAttestato = repository ?? throw new ArgumentNullException(nameof(repository));

            BDAttestati = new ObservableCollection<Attestati>();
            FilteredBDAttestati = new ObservableCollection<Attestati>();
            _attestatiModificati = new HashSet<int>();

            CaricaDati();
        }

        #endregion

        #region Methods

        // ✅ Unico metodo che accede al DB
        private void CaricaDati()
        {
            try
            {
                var attestati = _repositoryAttestato.GetAll();

                foreach (var attestato in BDAttestati)
                    attestato.PropertyChanged -= Attestato_PropertyChanged;

                BDAttestati.Clear();
                _attestatiModificati.Clear();

                foreach (var attestato in attestati)
                {
                    attestato.PropertyChanged += Attestato_PropertyChanged;
                    BDAttestati.Add(attestato);
                }

                // ✅ All'avvio mostra solo i flaggati (Attivo = true)
                _filtroAttivoCorrente = FiltroAttivoEnum.SoloNonAttivi;
                FiltroRicerca = string.Empty;
                ApplicaFiltro();

                CommandManager.InvalidateRequerySuggested();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante il caricamento dei dati: {ex.Message}",
                    "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Attestato_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Attestati attestatoModificato && e.PropertyName == nameof(Attestati.Attivo))
            {
                if (!_attestatiModificati.Contains(attestatoModificato.Id))
                {
                    _attestatiModificati.Add(attestatoModificato.Id);
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private void SalvaModifiche()
        {
            try
            {
                if (_attestatiModificati.Count == 0)
                {
                    MessageBox.Show("Nessuna modifica da salvare.", "Informazione",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                int contatoreAggiornamenti = 0;

                foreach (var attestato in BDAttestati.Where(a => _attestatiModificati.Contains(a.Id)))
                {
                    _repositoryAttestato.Update(attestato);
                    contatoreAggiornamenti++;
                }

                MessageBox.Show(contatoreAggiornamenti == 1
                        ? "Salvataggio corretto!"
                        : $"Salvataggio di {contatoreAggiornamenti} modifiche",
                    "Salvataggio Completato",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                _attestatiModificati.Clear();
                CommandManager.InvalidateRequerySuggested();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante il salvataggio: {ex.Message}",
                    "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetFiltro()
        {
            FiltroRicerca = string.Empty;
            _filtroAttivoCorrente = FiltroAttivoEnum.Tutti;
            ApplicaFiltro();
        }

        // ✅ Imposta il filtro e applica SENZA ricaricare dal DB
        private void ImpostaFiltroAttivo(FiltroAttivoEnum filtro)
        {
            _filtroAttivoCorrente = filtro;
            ApplicaFiltro();
        }

        // ✅ Lavora SOLO sulla collection in memoria BDAttestati
        private void ApplicaFiltro()
        {
            FilteredBDAttestati.Clear();

            IEnumerable<Attestati> risultati = BDAttestati;

            // Filtro testuale
            if (!string.IsNullOrWhiteSpace(FiltroRicerca))
            {
                var filtro = FiltroRicerca.ToLower();

                risultati = risultati.Where(a =>
                    a.Id.ToString().Contains(filtro) ||
                    (!string.IsNullOrEmpty(a.MatricolaDipendente) && a.MatricolaDipendente.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.Dipendente) && a.Dipendente.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.TitoloCorso) && a.TitoloCorso.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.CodiceAttivitaFormativa) && a.CodiceAttivitaFormativa.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.CodiceMateriaCorso) && a.CodiceMateriaCorso.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.EnteFormatore) && a.EnteFormatore.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.DenominazioneEnteCertificatore) && a.DenominazioneEnteCertificatore.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.DataInizioCorso) && a.DataInizioCorso.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.DataFineCorso) && a.DataFineCorso.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.AnnoCorso) && a.AnnoCorso.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.ValiditaAnni) && a.ValiditaAnni.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.DataScadenzaCorso) && a.DataScadenzaCorso.Contains(filtro)) ||
                    (!string.IsNullOrEmpty(a.LinkAttestato) && a.LinkAttestato.ToLower().Contains(filtro))
                );
            }

            // ✅ Filtro Attivo
            switch (_filtroAttivoCorrente)
            {
                case FiltroAttivoEnum.SoloAttivi:
                    risultati = risultati.Where(a => a.Attivo == true);
                    break;
                case FiltroAttivoEnum.SoloNonAttivi:
                    risultati = risultati.Where(a => a.Attivo == false || a.Attivo == null);
                    break;
                case FiltroAttivoEnum.Tutti:
                default:
                    break;
            }

            foreach (var attestato in risultati)
                FilteredBDAttestati.Add(attestato);
        }

        private void ApriDocumento(Attestati attestato)
        {
            _tool.ApriDocumento(attestato);
        }

        //**
        // ✅ Passa i dati filtrati (o tutti se non c'è filtro)
        private void EsportaInCsv()
        {
            var dati = FilteredBDAttestati?.Any() == true
                ? FilteredBDAttestati
                : BDAttestati;

            _tool.EsportaInCsv(dati);
        }

        private string EscapaCsv(string valore)
        {
            if (string.IsNullOrEmpty(valore)) return "";
            if (valore.Contains(";") || valore.Contains(",") || valore.Contains("\"") || valore.Contains("\n"))
            {
                valore = valore.Replace("\"", "\"\"");
                return $"\"{valore}\"";
            }
            return valore;
        }

        public override void Cleanup()
        {
            if (BDAttestati != null)
            {
                foreach (var attestato in BDAttestati)
                    attestato.PropertyChanged -= Attestato_PropertyChanged;
            }
            base.Cleanup();
        }

        #endregion

        #region Enums

        private enum FiltroAttivoEnum
        {
            Tutti,
            SoloAttivi,
            SoloNonAttivi
        }

        #endregion
    }
}