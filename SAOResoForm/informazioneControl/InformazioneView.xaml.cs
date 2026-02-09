using SAOResoForm.informazioneControl;
using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using System.Collections.Generic;
using System.Windows;

namespace SAOResoForm.informazioneControl
{
    public partial class InformazioneView : Window
    {
        public InformazioneView()
        {
            InitializeComponent();
            DataContext = new InformazioneViewModel(new RepositoryService(), new Tool());
        }
    }
}
