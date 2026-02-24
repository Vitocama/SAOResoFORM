using SAOResoForm.LoginControl;
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
            var repositoryService = new RepositoryService();
            var tool = new Tool();
            var appServices = new AppServices(repositoryService, tool);
            DataContext = new MainViewModel(appServices);

            Loaded += (s, e) =>
            {
                var loginView = new LoginView { Owner = this };
                if (loginView.ShowDialog() != true)
                {
                    Close();
                }
            };
        }
    }
}