using SAOResoForm.LoginControl;
using SAOResoForm.Service.App;
using SAOResoForm.Service.IdentityService;
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
            var repositoryService = new RepositoryService();
            var tool = new Tool();
            var appServices = new AppServices(repositoryService, tool);
            var mainViewModel = new MainViewModel(appServices);
            DataContext = mainViewModel;

            Loaded += (s, e) =>
            {
                var identity = new Identity();
                var loginVm = new LoginViewModel(identity);
                var loginView = new LoginView(loginVm) { Owner = this };

                if (loginView.ShowDialog() != true)
                {
                    Close();
                }
                else
                {
                    // Login riuscito: aggiorna i permessi del menu
                    mainViewModel.MenuVM.AggiornaPemessi();
                }
            };
        }
    }
}