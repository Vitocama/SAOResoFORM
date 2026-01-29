using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.AttestatiControl.AttestazioniInserimentoControl;
using SAOResoForm.InserimentoControl;
using SAOResoForm.Models;
using SAOResoForm.ModificaControl;
using SAOResoForm.Service.App;
using SAOResoForm.VisualizzaControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl
{
    public class AttestatiViewModel : INotifyPropertyChanged

    {

        private readonly MainViewModel _mainVM;
        private readonly AppServices _appServices;

        public ICommand AttestatiInseriesciCommand { get; }
        public ICommand AttestatiVisualizzaCommand { get; }


        public AttestatiViewModel(MainViewModel mainVM, AppServices appServices)
        {
            _mainVM = mainVM;
            _appServices = appServices;

            AttestatiInseriesciCommand = new RelayCommand(attestatiInseriesciCommand);
            AttestatiVisualizzaCommand = new RelayCommand(attestatiVisualizzaCommand);

        }

        private void attestatiVisualizzaCommand()
        {
            throw new NotImplementedException();
        }

        private void attestatiInseriesciCommand()
        {
          
            var window = new AttestatiInserimentoView(_mainVM,_appServices);
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog(); // oppure Show()
        }

        

        
        
       
    




        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
