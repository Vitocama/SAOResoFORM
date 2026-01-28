using SAOResoForm.Service.App;
using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using System.Windows;

namespace SAOResoForm
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Crea i servizi
            var repositoryService = new RepositoryService();
            var tool = new Tool();

            // Raggruppa in AppServices
            var appServices = new AppServices(repositoryService, tool);

            // Passa AppServices al MainViewModel
            DataContext = new MainViewModel(appServices);
        }
    }
}