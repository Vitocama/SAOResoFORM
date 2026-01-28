using System.Windows;
using SAOResoForm.Models;
using SAOResoForm.Service.Repository;

namespace SAOResoForm.ModificaControl
{
    public partial class ModificaView : Window
    {
        public ModificaView(Personale itemToEdit, IRepositoryService repository)
        {
            InitializeComponent();

            var viewModel = new ModificaViewModel(itemToEdit, repository);

            viewModel.DatiAggiornati += (s, e) =>
            {
                this.DialogResult = true;
                this.Close();
            };

            DataContext = viewModel;
        }
    }
}