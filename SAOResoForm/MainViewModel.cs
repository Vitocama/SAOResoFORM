using SAOResoForm.HomeControl;
using SAOResoForm.MenuControl;
using SAOResoForm.Service.App;
using SAOResoForm.Service.IdentityService;
using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SAOResoForm
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AppServices _services;
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public MenuViewModel MenuVM { get; }

        public MainViewModel(AppServices services)
        {
            _services = services;
            MenuVM = new MenuViewModel(this, services, new Identity());
            CurrentViewModel = new HomeViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}