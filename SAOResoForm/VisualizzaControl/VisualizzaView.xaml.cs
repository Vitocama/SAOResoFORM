using SAOResoForm.Models;
using SAOResoForm.ModificaControl;
using SAOResoForm.Service.App;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SAOResoForm.VisualizzaControl
{
    public partial class VisualizzaView : Window
    {
        private readonly AppServices _services;
        private readonly VisualizzaViewModel _viewModel;

        public VisualizzaView(MainViewModel mainVM, AppServices services)
        {
            InitializeComponent();
            _services = services;

            _viewModel = new VisualizzaViewModel(mainVM, services);
            DataContext = _viewModel;
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid == null)
                return;

            if (dataGrid.SelectedItem == null)
                return;

            var personaleSelezionato = dataGrid.SelectedItem as Personale;
            if (personaleSelezionato == null)
                return;

            var modificaView = new ModificaView(
                personaleSelezionato,
                _services.RepositoryService
            );

            if (modificaView.ShowDialog() == true)
            {
                _viewModel.CaricaPersonale();
            }
        }
    }
}