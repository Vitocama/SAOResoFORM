using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.CreaAccountControl;
using SAOResoForm.DBScelta;
using SAOResoForm.HomeControl;
using SAOResoForm.informazioneControl;
using SAOResoForm.PersonaleControl;
using SAOResoForm.Reportistica;
using SAOResoForm.Service.App;
using SAOResoForm.Service.IdentityService;
using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using RelayCommand = GalaSoft.MvvmLight.CommandWpf.RelayCommand;

namespace SAOResoForm.MenuControl
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainVM;
        private readonly AppServices _services;
        private readonly IIdentity _identity;
        private readonly IRepositoryService _repositoryService;
        private readonly ITool _tool;

        private InformazioneView _informazioneView;
        private ReportisticaView _reportisticaView;
        private SceltaDBView _dbSceltaView;
        private CreaAccountView _creaAccountView; // ← corretto con underscore

        // ── INotifyPropertyChanged ────────────────────────────────
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // ── Permessi ──────────────────────────────────────────────
        private bool _isMaster;
        public bool IsMaster
        {
            get => _isMaster;
            private set { _isMaster = value; OnPropertyChanged(); }
        }

        // ── Comandi ───────────────────────────────────────────────
        public ICommand OpenHomeCommand { get; }
        public ICommand OpenPersonaleCommand { get; }
        public ICommand OpenAttestatiCommand { get; }
        public ICommand OpenReportisticaCommand { get; }
        public ICommand openSceltaDBCommand { get; }
        public ICommand logout { get; }
        public ICommand creaAccount { get; }

        // ── Costruttore ───────────────────────────────────────────
        public MenuViewModel(MainViewModel mainVM, AppServices services, IIdentity identity)
        {
            _mainVM = mainVM;
            _services = services;
            _identity = identity;
            _repositoryService = services.RepositoryService;
            _tool = services.Tool;

            IsMaster = false;

            OpenHomeCommand = new RelayCommand(() => _mainVM.CurrentViewModel = new HomeViewModel());
            OpenPersonaleCommand = new RelayCommand(OpenPersonaleCommandExecute);
            OpenAttestatiCommand = new RelayCommand(OpenAttestati);
            OpenReportisticaCommand = new RelayCommand(OpenReportistica);
            openSceltaDBCommand = new RelayCommand(OpenSceltaDB);
            logout = new RelayCommand(() => _identity.logout());
            creaAccount = new RelayCommand(OpenCreaAccount);
        }

        // ── Aggiorna permessi dopo il login ───────────────────────
        public void AggiornaPemessi()
        {
            IsMaster = Identity.SessionManager.Ruolo?.ToLower() == "admin";
        }

        // ── Metodi privati ────────────────────────────────────────
        private void OpenCreaAccount()
        {
            if (_creaAccountView == null || !_creaAccountView.IsVisible)
            {
                _creaAccountView = new CreaAccountView();
                _creaAccountView.Closed += (s, e) => _creaAccountView = null;
                _creaAccountView.Show();
            }
            else
            {
                if (_creaAccountView.WindowState == WindowState.Minimized)
                    _creaAccountView.WindowState = WindowState.Normal;
                _creaAccountView.Activate();
                _creaAccountView.Focus();
            }
        }

        private void OpenReportistica()
        {
            if (_reportisticaView == null || !_reportisticaView.IsVisible)
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