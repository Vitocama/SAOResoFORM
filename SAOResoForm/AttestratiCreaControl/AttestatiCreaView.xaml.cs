using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.AttestratiCreaControl
{
    public partial class AttestatiCreaView : Window
    {
        public AttestatiCreaView()
        {
            InitializeComponent();
        }

        // Validazione solo numeri per Validità Anni
        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        private bool IsTextNumeric(string text)
        {
            return Regex.IsMatch(text, "^[0-9]+$");
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}