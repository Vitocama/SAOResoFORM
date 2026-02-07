using System;
using System.Windows;

namespace SAOResoForm.AttestratiCreaControl
{
    /// <summary>
    /// Interaction logic for AttestatiCreaView.xaml
    /// </summary>
    public partial class AttestatiCreaView : Window
    {
        private readonly AttestatiCreaViewModel _viewModel;

        // ========================
        // CONSTRUCTOR
        // ========================
        public AttestatiCreaView(AttestatiCreaViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = _viewModel;

            SubscribeToViewModelEvents();
        }

        // ========================
        // SUBSCRIBE EVENTS
        // ========================
        private void SubscribeToViewModelEvents()
        {
            _viewModel.RichiediChiusura += OnRichiediChiusura;
            _viewModel.MostraMessaggioSuccesso += OnMostraMessaggioSuccesso;
            _viewModel.MostraMessaggioErrore += OnMostraMessaggioErrore;
            _viewModel.RichiediConfermaAnnullamento += OnRichiediConfermaAnnullamento;
        }

        // ========================
        // EVENT HANDLERS
        // ========================
        private void OnRichiediChiusura(object sender, EventArgs e)
        {
            Close();
        }

        private void OnMostraMessaggioSuccesso(object sender, string messaggio)
        {
            MessageBox.Show(
                messaggio,
                "Operazione completata",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void OnMostraMessaggioErrore(object sender, string messaggio)
        {
            MessageBox.Show(
                messaggio,
                "Errore",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void OnRichiediConfermaAnnullamento(object sender, bool _)
        {
            var result = MessageBox.Show(
                "Sei sicuro di voler annullare?\nTutti i dati inseriti andranno persi.",
                "Conferma annullamento",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _viewModel.ConfermaAnnullamento();
            }
        }

        // ========================
        // CLEANUP
        // ========================
        private void UnsubscribeFromViewModelEvents()
        {
            if (_viewModel == null)
                return;

            _viewModel.RichiediChiusura -= OnRichiediChiusura;
            _viewModel.MostraMessaggioSuccesso -= OnMostraMessaggioSuccesso;
            _viewModel.MostraMessaggioErrore -= OnMostraMessaggioErrore;
            _viewModel.RichiediConfermaAnnullamento -= OnRichiediConfermaAnnullamento;
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeFromViewModelEvents();
            base.OnClosed(e);
        }
    }
}
