using SAOResoForm.AttestatiControl.AttestatiCreaControl;
using SAOResoForm.Service.App;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl.AttestazioniInserimentoControl
{
    public partial class AttestatiInserimentoView : Window
    {
        private readonly AttestatiInserimentoViewModel _viewModel;

        public AttestatiInserimentoView(MainViewModel mainVM, AppServices appServices)
        {
            InitializeComponent();

            _viewModel = new AttestatiInserimentoViewModel(mainVM, appServices);
            DataContext = _viewModel;
        }

        // DOPPIO CLICK SUL DATAGRID
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel?.PersonaleSelezionato == null)
                return;

            // ✅ APRI SOLO LA VIEW GIUSTA
  
            // refresh dati
            _viewModel.AggiornaDati();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _viewModel?.Dispose();
        }
    }
}
