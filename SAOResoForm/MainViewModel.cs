using SAOResoForm.HomeControl;
using SAOResoForm.MenuControl;
using SAOResoForm.Service.App;
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

        public MainViewModel(AppServices services)  // ← AGGIUNGI IL PARAMETRO
        {
            _services = services;  // ← ASSEGNA IL PARAMETRO
            MenuVM = new MenuViewModel(this, _services);
            CurrentViewModel = new HomeViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}