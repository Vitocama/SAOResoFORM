using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.AttestatiControl;
using SAOResoForm.AttestatiControl.AttestazioniInserimentoControl;
using SAOResoForm.HomeControl;
using SAOResoForm.informazioneControl;
using SAOResoForm.PersonaleControl;
using SAOResoForm.Service.App;
using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using System;
using System.Windows;
using System.Windows.Input;
using RelayCommand = GalaSoft.MvvmLight.CommandWpf.RelayCommand;


namespace SAOResoForm.MenuControl
{
    public class MenuViewModel
    {
        private readonly MainViewModel _mainVM;
        private readonly AppServices _services;

        // Riferimenti diretti per comodità (opzionale)
        private readonly IRepositoryService _repositoryService;
        private readonly ITool _tool;
        private  InformazioneView _informazioneView;

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

            OpenAttestatiCommand = new RelayCommand(OpenAttestati);
           
        }

        private void OpenAttestati()
        {
            if (_informazioneView == null || !_informazioneView.IsVisible)
            {
                _informazioneView = new InformazioneView();
                _informazioneView.Closed += (s, e) => _informazioneView = null;
                _informazioneView.Show();
            }
            else
            {
                if (_informazioneView.WindowState == WindowState.Minimized)
                    _informazioneView.WindowState = WindowState.Normal;
                _informazioneView.Activate();
                _informazioneView.Focus();
            }
        }

        private void OpenPersonaleCommandExecute()
        {
            // Passa AppServices invece dei singoli servizi
            _mainVM.CurrentViewModel = new PersonaleViewModel(_mainVM, _services);
        }
    }
}