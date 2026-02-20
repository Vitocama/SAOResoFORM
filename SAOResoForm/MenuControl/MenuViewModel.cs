using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.AttestatiControl;
using SAOResoForm.AttestatiControl.AttestazioniInserimentoControl;
using SAOResoForm.DBScelta;
using SAOResoForm.HomeControl;
using SAOResoForm.informazioneControl;
using SAOResoForm.PersonaleControl;
using SAOResoForm.Reportistica;
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
        private  ReportisticaView _reportisticaView;
        private  SceltaDBView _dbSceltaView;    

        public ICommand OpenHomeCommand { get; }
        public ICommand OpenPersonaleCommand { get; }
        public ICommand OpenAttestatiCommand { get; }

        public ICommand OpenReportisticaCommand { get; }

        public ICommand openSceltaDBCommand { get; }

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

            OpenReportisticaCommand = new RelayCommand(OpenReportistica);

            openSceltaDBCommand=new RelayCommand(OpenSceltaDB);


        }

        private void OpenReportistica()
        {
            if(_reportisticaView == null || !_reportisticaView.IsVisible)
{
                _reportisticaView = new ReportisticaView();

                _reportisticaView.Closed += (s, e) => _reportisticaView = null;
                _reportisticaView.Show();
            }
else
            {
                if (_reportisticaView.WindowState == WindowState.Minimized)
                    _reportisticaView.WindowState = WindowState.Normal;

                _reportisticaView.Activate();
                _reportisticaView.Focus();
            }

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

        private void OpenSceltaDB()
        {
            if (_dbSceltaView == null || !_dbSceltaView.IsVisible)
            {
                _dbSceltaView = new SceltaDBView();
                _dbSceltaView.Closed += (s, e) => _dbSceltaView = null;
                _dbSceltaView.Show();
            }
            else
            {
                if (_dbSceltaView.WindowState == WindowState.Minimized)
                    _dbSceltaView.WindowState = WindowState.Normal;
                _dbSceltaView.Activate();
                _dbSceltaView.Focus();
            }
        }

    }
}