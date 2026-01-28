using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SAOResoForm.HomeControl
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        // Aggiungi qui le proprietà necessarie per la tua Home
        // Esempio:
        // private string _welcomeMessage;
        // public string WelcomeMessage
        // {
        //     get => _welcomeMessage;
        //     set { _welcomeMessage = value; OnPropertyChanged(); }
        // }

        public HomeViewModel()
        {
            // Inizializzazione
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}