using System.Windows;

namespace SAOResoForm.LoginControl
{
    public partial class LoginView : Window
    {
        private readonly LoginViewModel _vm;

        public LoginView(LoginViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            _vm.LoginSucceeded += (sender, args) =>
            {
                DialogResult = true;
                Close();
            };

            _vm.RichiestaChiusura += (sender, args) =>
            {
                DialogResult = false;
                Close();
            };
        }

        // PASSWORD ATTUALE
        private void PwdPassword_PasswordChanged(object sender, RoutedEventArgs e)
            => _vm.Password = PwdPassword.Password;

        // NUOVA PASSWORD
        private void PwdNuova_PasswordChanged(object sender, RoutedEventArgs e)
            => _vm.NuovaPassword = PwdNuova.Password;

        // CONFERMA PASSWORD
        private void PwdConferma_PasswordChanged(object sender, RoutedEventArgs e)
            => _vm.ConfermaPassword = PwdConferma.Password;
    }
}