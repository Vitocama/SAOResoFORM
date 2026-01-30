using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl.AttestatiCreaControl  // ← CORRETTO
{
    /// <summary>
    /// Logica di interazione per AttestatiCreaView.xaml
    /// </summary>
    public partial class AttestatiCreaView : Window
    {
        public AttestatiCreaView()
        {
            InitializeComponent();
        }

        // ========================
        // VALIDAZIONE SOLO NUMERI
        // ========================
        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        private bool IsTextNumeric(string text)
        {
            return Regex.IsMatch(text, "^[0-9]+$");
        }

        // ========================
        // PULSANTE ANNULLA
        // ========================
        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            var risultato = MessageBox.Show(
                "Sei sicuro di voler annullare? I dati inseriti andranno persi.",
                "Conferma Annullamento",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (risultato == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        // ========================
        // CLEANUP
        // ========================
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            (DataContext as IDisposable)?.Dispose();
        }
    }
}