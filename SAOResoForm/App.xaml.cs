using SAOResoForm.LoginControl;
using SAOResoForm.Service.IdentityService;
using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using SAOResoForm.Service.App;
using System.Windows;

namespace SAOResoForm
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Crea il servizio di identità e il ViewModel del login
            var identity = new Identity();
            var loginVm = new LoginViewModel(identity);
            var login = new LoginView(loginVm);

            // Mostra il login — se va a buon fine apre il MainWindow
            if (login.ShowDialog() == true)
            {
                var repositoryService = new RepositoryService();
                var tool = new Tool();
                var appServices = new AppServices(repositoryService, tool);

                var main = new MainWindow();
                main.DataContext = new MainViewModel(appServices);
                MainWindow = main;
                main.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}