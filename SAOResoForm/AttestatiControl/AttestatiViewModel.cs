using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.AttestatiControl.AttestazioniInserimentoControl;
using SAOResoForm.informazioneControl;
using SAOResoForm.Service.App;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using RelayCommand = GalaSoft.MvvmLight.CommandWpf.RelayCommand;


namespace SAOResoForm.AttestatiControl
{
    public class AttestatiViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainVM;
        private readonly AppServices _appServices;

        public ICommand AttestatiInseriesciCommand { get; }
        public ICommand InformazioneCommand { get; }

        public AttestatiViewModel(MainViewModel mainVM, AppServices appServices)
        {
            _mainVM = mainVM;
            _appServices = appServices;

            AttestatiInseriesciCommand = new RelayCommand(AttestatiInseriesciCommand_Execute);
            InformazioneCommand = new RelayCommand(InformazioneCommand_Execute);
        }

        private void InformazioneCommand_Execute()
        {
            // CORRETTO: Passa AppServices al costruttore
            var window = new InformazioneView();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void AttestatiInseriesciCommand_Execute()
        {
            var window = new AttestatiInserimentoView(_mainVM, _appServices);
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}