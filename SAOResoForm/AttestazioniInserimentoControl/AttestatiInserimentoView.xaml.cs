using SAOResoForm.Service.App;
using System;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl.AttestazioniInserimentoControl
{
    /// <summary>
    /// Logica di interazione per AttestatiInserimentoView.xaml
    /// </summary>
    public partial class AttestatiInserimentoView : Window
    {
        private AttestatiInserimentoViewModel _viewModel;

        // ========================
        // COSTRUTTORE CON PARAMETRI
        // ========================
        public AttestatiInserimentoView(MainViewModel mainVM, AppServices appServices)
        {
            InitializeComponent();

            // IMPORTANTE: Crea e imposta il DataContext
            _viewModel = new AttestatiInserimentoViewModel(mainVM, appServices);
            this.DataContext = _viewModel;
        }

        // ========================
        // DOUBLE CLICK
        // ========================
        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel?.PersonaleSelezionato != null)
            {
                MessageBox.Show(
                    $"Inserimento attestato per:\n{_viewModel.PersonaleSelezionato.Cognome} {_viewModel.PersonaleSelezionato.Nome}",
                    "Inserimento Attestato",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        // ========================
        // CLEANUP
        // ========================
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _viewModel?.Dispose();
        }
    }
}