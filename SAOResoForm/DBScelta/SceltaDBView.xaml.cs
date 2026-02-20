using System.Windows;

namespace SAOResoForm.DBScelta
{
    public partial class SceltaDBView : Window
    {
        public SceltaDBView()
        {
            InitializeComponent();
            DataContext = new SceltaDBViewModel();
        }
    }
}