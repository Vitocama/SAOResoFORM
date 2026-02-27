using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.Models;
using SAOResoForm.Service.Repository;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.CreaAccountControl
{
    public class CreaAccountViewModel : INotifyPropertyChanged
    {
        private const string PASSWORD_DEFAULT = "Aquilone.000";
        private readonly IRepositoryService _repositoryService;

        // ── Proprietà ─────────────────────────────────────────────
        private string _utente;
        public string Utente
        {
            get => _utente;
            set { _utente = value; OnPropertyChanged(); }
        }

        private string _cognome;
        public string Cognome
        {
            get => _cognome;
            set { _cognome = value; OnPropertyChanged(); }
        }

        private string _nome;
        public string Nome
        {
            get => _nome;
            set { _nome = value; OnPropertyChanged(); }
        }

        private string _incarico;
        public string Incarico
        {
            get => _incarico;
            set { _incarico = value; OnPropertyChanged(); }
        }

        private string _matricola;
        public string Matricola
        {
            get => _matricola;
            set { _matricola = value; OnPropertyChanged(); }
        }

        private string _uuoo;
        public string Uuoo
        {
            get => _uuoo;
            set { _uuoo = value; OnPropertyChanged(); }
        }

        private string _ruolo;
        public string Ruolo
        {
            get => _ruolo;
            set { _ruolo = value; OnPropertyChanged(); }
        }

        private bool _amministratore;
        public bool Amministratore
        {
            get => _amministratore;
            set { _amministratore = value; OnPropertyChanged(); }
        }

        private bool _consultazione;
        public bool Consultazione
        {
            get => _consultazione;
            set { _consultazione = value; OnPropertyChanged(); }
        }

        public string Password => PASSWORD_DEFAULT;

        // ── Comandi ───────────────────────────────────────────────
        public ICommand SalvaCommand { get; }
        public ICommand AnnullaCommand { get; }

        // ── Costruttore ───────────────────────────────────────────
        public CreaAccountViewModel(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
            SalvaCommand = new RelayCommand(Salva);
            AnnullaCommand = new RelayCommand(Annulla);
        }

        // ── Salva ─────────────────────────────────────────────────
        private void Salva()
        {
            if (string.IsNullOrWhiteSpace(Utente))
            { MessageBox.Show("Il campo Utente è obbligatorio!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (string.IsNullOrWhiteSpace(Cognome))
            { MessageBox.Show("Il campo Cognome è obbligatorio!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (string.IsNullOrWhiteSpace(Nome))
            { MessageBox.Show("Il campo Nome è obbligatorio!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (string.IsNullOrWhiteSpace(Incarico))
            { MessageBox.Show("Il campo Incarico è obbligatorio!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (string.IsNullOrWhiteSpace(Matricola))
            { MessageBox.Show("Il campo Matricola è obbligatoria!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (string.IsNullOrWhiteSpace(Uuoo))
            { MessageBox.Show("Il campo UUOO è obbligatorio!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            if (string.IsNullOrWhiteSpace(Ruolo))
            { MessageBox.Show("Il campo Ruolo è obbligatorio!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

           Random random = new Random();

            int indice=random.Next(999999999);

            var nuovoAccount = new AccountUtenti
            { Id = indice,
                Utente = Utente,
                Password = PASSWORD_DEFAULT,
                Cognome = Cognome,
                Nome = Nome,
                Incarico = Incarico,
                Uuoo = Uuoo,
                Ruolo = Ruolo
            };

            string risultato = _repositoryService.SaveAccount(nuovoAccount);
            MessageBox.Show(risultato, "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            if (risultato.Contains("successo"))
                Annulla();
        }

        // ── Annulla ───────────────────────────────────────────────
        private void Annulla()
        {
            Utente = string.Empty;
            Cognome = string.Empty;
            Nome = string.Empty;
            Incarico = string.Empty;
            Matricola = string.Empty;
            Uuoo = string.Empty;
            Ruolo = string.Empty;
            Amministratore = false;
            Consultazione = false;
        }

        // ── INotifyPropertyChanged ────────────────────────────────
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}