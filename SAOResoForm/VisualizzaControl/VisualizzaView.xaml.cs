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

        public VisualizzaView(MainViewModel mainVM, AppServices services)
        {
            InitializeComponent();
            _services = services;
            DataContext = new VisualizzaViewModel(mainVM, services);
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Verifica che il sender sia un DataGrid
            var dataGrid = sender as DataGrid;
            if (dataGrid == null)
                return;


            // Recupera l'elemento selezionato
            if (dataGrid.SelectedItem == null)
                return;

            // Cast al tuo modello (sostituisci Personale se il nome è diverso)
            var personaleSelezionato = dataGrid.SelectedItem as Personale;
            if (personaleSelezionato == null)
                return;

            // Apertura finestra di modifica
            var modificaView = new ModificaView(
                personaleSelezionato,
                _services.RepositoryService   // o quello corretto
            );

            modificaView.ShowDialog();
        }
    }
}
