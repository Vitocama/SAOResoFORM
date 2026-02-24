using SAOResoForm.Service.IdentityService;
using System.Windows;

namespace SAOResoForm.LoginControl
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();

            var vm = new LoginViewModel(new Identity());
            vm.LoginSucceeded += (s, e) => { DialogResult = true; Close(); };
            vm.RichiestaChiusura += (s, e) => { DialogResult = false; Close(); };

            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(LoginViewModel.MostraPassword))
                    if (!vm.MostraPassword)
                        PwdPassword.Password = vm.Password;  // occhio chiuso → aggiorna PasswordBox
            };

            DataContext = vm;
        }

        private void PwdPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = PwdPassword.Password;
        }
    }
}