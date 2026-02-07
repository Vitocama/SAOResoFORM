using System.Windows;
using SAOResoForm.Service.App;
using SAOResoForm.Service.Repository;

namespace SAOResoForm.informazioneControl
{
    public partial class InformazioneView : Window
    {
        public InformazioneView()
        {
            InitializeComponent();
            DataContext = new InformazioneViewModel();
        }
    }
}