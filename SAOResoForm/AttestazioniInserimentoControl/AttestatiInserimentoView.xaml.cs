
using SAOResoForm.AttestatiControl.AttestatiCreaControl;

using SAOResoForm.Service.App;
using System;
using System.Windows;
using System.Windows.Input;
using AttestatiCreaView = SAOResoForm.AttestatiControl.AttestatiCreaControl.AttestatiCreaView;

namespace SAOResoForm.AttestatiControl.AttestazioniInserimentoControl
{
    public partial class AttestatiInserimentoView : Window
    {
        private AttestatiInserimentoViewModel _viewModel;

        public AttestatiInserimentoView(MainViewModel mainVM, AppServices appServices)
        {
            InitializeComponent();

            _viewModel = new AttestatiInserimentoViewModel(mainVM, appServices);
            this.DataContext = _viewModel;
        }

        // DOPPIO CLICK SUL DATAGRID
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel?.PersonaleSelezionato != null)
            {
                // Apri la finestra di creazione passando l'oggetto Personale
                var creaView = new AttestatiCreaView();
                creaView.DataContext = new AttestatiCreaViewModel(
                    _viewModel.PersonaleSelezionato,  // Passa l'oggetto completo
                    _viewModel._appServices
                );
                creaView.Owner = this;
                creaView.ShowDialog();

                // Aggiorna dopo la chiusura
                _viewModel.AggiornaDati();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _viewModel?.Dispose();
        }
    }
}