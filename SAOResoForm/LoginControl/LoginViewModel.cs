using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;

namespace SAOResoForm.LoginControl
{
    public class LoginViewModel : ViewModelBase
    {
        public event EventHandler LoginSucceeded;

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

        public string Password { get; set; } = string.Empty;

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

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(
                execute: () => EseguiLogin(),
                canExecute: () => !string.IsNullOrWhiteSpace(Username) && !IsBusy
            );
        }

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
        {
            // ESEMPIO STATICO — sostituisci con query EF/DB reale
            return username == "admin" && password == "1234";
        }
    }
}