using System.Windows;
using SAOResoForm.Models;
using SAOResoForm.Service.Repository;

namespace SAOResoForm.ModificaControl
{
    public partial class ModificaView : Window
    {
        // Costruttore corretto con corpo
        public ModificaView(Personale itemToEdit, IRepositoryService repository)
        {
            InitializeComponent(); // carica il XAML
            DataContext = new ModificaViewModel(itemToEdit, repository);
        }
    }
}
