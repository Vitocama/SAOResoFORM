using SAOResoForm.Repositories;
using SAOResoForm.Service.Repository.tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAOResoForm.Reportistica
{
    /// <summary>
    /// Logica di interazione per ReportisticaView.xaml
    /// </summary>
    public partial class ReportisticaView : Window
    {
        public ReportisticaView()
        {
            InitializeComponent();
            DataContext = new ReportisticaViewModel(new RepositoryAttestato(), new Tool()
                );
        }
    }
}
