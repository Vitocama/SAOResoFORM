using System.Windows;
using System.Windows.Input;
using SAOResoForm.Services;

namespace SAOResoForm.Views
{
    public partial class PasswordDialog : Window
    {
        private readonly CartellaRiservataService _servizio;
        public CartellaRiservataService Servizio => _servizio;

        public PasswordDialog()
        {
            InitializeComponent();
            _servizio = new CartellaRiservataService();
            Loaded += (s, e) => TxtPassword.Focus();
        }

        private void BtnAccedi_Click(object sender, RoutedEventArgs e)
        {
            Accedi();
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Accedi();
        }

        private void Accedi()
        {
            string password = TxtPassword.Password;

            if (string.IsNullOrEmpty(password))
            {
                TxtErrore.Text = "Inserisci la password.";
                TxtErrore.Visibility = Visibility.Visible;
                return;
            }



        }

        private void BtnAnnulla_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
