using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.InserimentoControl;
using SAOResoForm.Service.App;
using SAOResoForm.VisualizzaControl;
using System.Data.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
namespace SAOResoForm.PersonaleControl
{
    public class PersonaleViewModel
    {
        private readonly MainViewModel _mainVM;
        private readonly AppServices _appServices;
        public ICommand OpenInserimentoCommand { get; }
        public ICommand OpenVisualizzaCommand { get; }

        public PersonaleViewModel(MainViewModel mainVM, AppServices appServices)
        {
            _mainVM = mainVM;
            _appServices = appServices;
            OpenInserimentoCommand = new RelayCommand(OpenInserimento);
            OpenVisualizzaCommand = new RelayCommand(OpenVisualizza);

        }
        private void OpenInserimento()
        {
            _mainVM.CurrentViewModel = new InserimentoViewModel(_mainVM, _appServices);
        }
        private void OpenVisualizza()
        {
            var visualizzaWindow = new VisualizzaView(_mainVM, _appServices);
            visualizzaWindow.DataContext = new VisualizzaViewModel(_mainVM, _appServices);
            visualizzaWindow.ShowDialog();
        }

    }
}
