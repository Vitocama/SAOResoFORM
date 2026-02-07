using SAOResoForm.AttestratiCreaControl;
using SAOResoForm.Models;
using SAOResoForm.Service.App;
using System;
using System.Windows;

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

            _viewModel.RichiediAperturaCreaAttestato += OnRichiediAperturaCreaAttestato;
        }

        private void OnRichiediAperturaCreaAttestato(object sender, Personale personale)
        {
            var vm = new AttestatiCreaViewModel(personale, _viewModel.AppServices);
            var view = new AttestatiCreaView(vm);

            view.ShowDialog();
        }


        protected override void OnClosed(EventArgs e)
        {
            _viewModel.RichiediAperturaCreaAttestato -= OnRichiediAperturaCreaAttestato;
            _viewModel.Dispose();
            base.OnClosed(e);
        }
    }
}
