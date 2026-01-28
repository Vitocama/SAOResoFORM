using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.HomeControl;
using SAOResoForm.PersonaleControl;
using SAOResoForm.AttestatiControl;
using SAOResoForm.Service.App;
using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using System.Windows.Input;

namespace SAOResoForm.MenuControl
{
    public class MenuViewModel
    {
        private readonly MainViewModel _mainVM;
        private readonly AppServices _services;

        // Riferimenti diretti per comodità (opzionale)
        private readonly IRepositoryService _repositoryService;
        private readonly ITool _tool;

        public ICommand OpenHomeCommand { get; }
        public ICommand OpenPersonaleCommand { get; }
        public ICommand OpenAttestatiCommand { get; }

        public MenuViewModel(MainViewModel mainVM, AppServices services)
        {
            _mainVM = mainVM;
            _services = services;

            // Estrai i servizi da AppServices
            _repositoryService = services.RepositoryService;
            _tool = services.Tool;

            OpenHomeCommand = new RelayCommand(() =>
            {
                _mainVM.CurrentViewModel = new HomeViewModel();
            });

            OpenPersonaleCommand = new RelayCommand(OpenPersonaleCommandExecute);

            OpenAttestatiCommand = new RelayCommand(() =>
            {
                _mainVM.CurrentViewModel = new AttestatiViewModel();
            });
        }

        private void OpenPersonaleCommandExecute()
        {
            // Passa AppServices invece dei singoli servizi
            _mainVM.CurrentViewModel = new PersonaleViewModel(_mainVM, _services);
        }
    }
}