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
        }

        private void PwdPassword_PasswordChanged(object sender, RoutedEventArgs e)
            => _vm.Password = PwdPassword.Password;
    }
}