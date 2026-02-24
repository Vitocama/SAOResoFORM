using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SAOResoForm.Service.IdentityService;
using System;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.LoginControl
{
    public class LoginViewModel : ViewModelBase
    {
        public event EventHandler LoginSucceeded;
        public event EventHandler RichiestaChiusura;

        private readonly IIdentity _iidentity;

        // ── USERNAME ──────────────────────────────────────────────
        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                Set(ref _username, value);
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        // ── PASSWORD ATTUALE ──────────────────────────────────────
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        // ── ERRORE ────────────────────────────────────────────────
        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                Set(ref _errorMessage, value);
                RaisePropertyChanged(nameof(HasError));
            }
        }
        public bool HasError => !string.IsNullOrEmpty(_errorMessage);

        // ── BUSY ──────────────────────────────────────────────────
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(ref _isBusy, value);
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        // ── MOSTRA/NASCONDI PASSWORD ──────────────────────────────
        private bool _mostraPassword;
        public bool MostraPassword
        {
            get => _mostraPassword;
            set
            {
                Set(ref _mostraPassword, value);
                RaisePropertyChanged(nameof(IconaOcchio));
            }
        }
        public string IconaOcchio => MostraPassword ? "🙈" : "👁";

        // ── CAMBIA PASSWORD ───────────────────────────────────────
        private bool _mostraCambiaPassword;
        public bool MostraCambiaPassword
        {
            get => _mostraCambiaPassword;
            set => Set(ref _mostraCambiaPassword, value);
        }

        private string _nuovaPassword = string.Empty;
        public string NuovaPassword
        {
            get => _nuovaPassword;
            set
            {
                Set(ref _nuovaPassword, value);
                ((RelayCommand)CambiaPasswordCommand).RaiseCanExecuteChanged();
            }
        }

        private string _confermaPassword = string.Empty;
        public string ConfermaPassword
        {
            get => _confermaPassword;
            set
            {
                Set(ref _confermaPassword, value);
                ((RelayCommand)CambiaPasswordCommand).RaiseCanExecuteChanged();
            }
        }

        // ── COMANDI ───────────────────────────────────────────────
        public ICommand LoginCommand { get; }
        public ICommand ChiudiCommand { get; }
        public ICommand TogglePasswordCommand { get; }
        public ICommand ToggleCambiaPasswordCommand { get; }
        public ICommand CambiaPasswordCommand { get; }

        // ── COSTRUTTORE ───────────────────────────────────────────
        public LoginViewModel(IIdentity iidentity)
        {
            _iidentity = iidentity ?? throw new ArgumentNullException(nameof(iidentity));

            LoginCommand = new RelayCommand(
                execute: () => EseguiLogin(),
                canExecute: () => !string.IsNullOrWhiteSpace(Username) && !IsBusy
            );

            ChiudiCommand = new RelayCommand(
                () => RichiestaChiusura?.Invoke(this, EventArgs.Empty)
            );

            TogglePasswordCommand = new RelayCommand(
                () => MostraPassword = !MostraPassword
            );

            ToggleCambiaPasswordCommand = new RelayCommand(() =>
            {
                MostraCambiaPassword = !MostraCambiaPassword;
                // Reset campi e messaggi quando si chiude la sezione
                if (!MostraCambiaPassword)
                {
                    NuovaPassword = string.Empty;
                    ConfermaPassword = string.Empty;
                    ErrorMessage = string.Empty;
                }
            });

            CambiaPasswordCommand = new RelayCommand(
                execute: () => EseguiCambiaPassword(),
                canExecute: () => !string.IsNullOrWhiteSpace(NuovaPassword)
                                  && !string.IsNullOrWhiteSpace(ConfermaPassword)
            );
        }

        // ── LOGIN ─────────────────────────────────────────────────
        private void EseguiLogin()
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            try
            {
                if (VerificaCredenziali(Username, Password))
                    LoginSucceeded?.Invoke(this, EventArgs.Empty);
                else
                    ErrorMessage = "Credenziali non valide. Riprovare.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool VerificaCredenziali(string username, string password)
            => _iidentity.Autenticato(username, password);

        // ── CAMBIA PASSWORD ───────────────────────────────────────
        private void EseguiCambiaPassword()
        {
            ErrorMessage = string.Empty;

            // Validazione username
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Inserire il nome utente.";
                return;
            }

            // Validazione password attuale
            if (!VerificaCredenziali(Username, Password))
            {
                ErrorMessage = "Credenziali attuali non valide.";
                return;
            }

            // Validazione lunghezza
            if (NuovaPassword.Length < 6)
            {
                ErrorMessage = "La nuova password deve avere almeno 6 caratteri.";
                return;
            }

            // Validazione corrispondenza
            if (NuovaPassword != ConfermaPassword)
            {
                ErrorMessage = "Le password non coincidono.";
                return;
            }

            // Chiamata al servizio
            bool aggiornata = _iidentity.CambiaPassword(Username, NuovaPassword);

            if (aggiornata)
            {
                MostraCambiaPassword = false;
                NuovaPassword = string.Empty;
                ConfermaPassword = string.Empty;
                ErrorMessage = string.Empty;

                MessageBox.Show(
                    "Password aggiornata con successo.",
                    "Successo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                ErrorMessage = "Errore durante l'aggiornamento della password.";
            }
        }
    }
}