using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SAOResoForm.Service.IdentityService;
using System;
using System.Windows.Input;

namespace SAOResoForm.LoginControl
{
    public class LoginViewModel : ViewModelBase
    {
        public event EventHandler LoginSucceeded;
        public event EventHandler RichiestaChiusura;

        private readonly IIdentity _iidentity;

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

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

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

        public ICommand LoginCommand { get; }
        public ICommand ChiudiCommand { get; }
        public ICommand TogglePasswordCommand { get; }

        public LoginViewModel(IIdentity iidentity)
        {
            _iidentity = iidentity ?? throw new ArgumentNullException(nameof(iidentity)); // ← corretto

            LoginCommand = new RelayCommand(
                execute: () => EseguiLogin(),
                canExecute: () => !string.IsNullOrWhiteSpace(Username) && !IsBusy
            );

            ChiudiCommand = new RelayCommand(() => RichiestaChiusura?.Invoke(this, EventArgs.Empty));

            TogglePasswordCommand = new RelayCommand(() => MostraPassword = !MostraPassword);
        }

        private void EseguiLogin()
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            try
            {
                if (VerificaCredenziali(Username, Password))  // ← ora ritorna bool
                    LoginSucceeded?.Invoke(this, EventArgs.Empty);
                else
                    ErrorMessage = "Credenziali non valide. Riprovare.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool VerificaCredenziali(string username, string password) // ← bool
        {
            bool autenticato = _iidentity.Autenticato(username, password);

            return autenticato;
        }
    }
}