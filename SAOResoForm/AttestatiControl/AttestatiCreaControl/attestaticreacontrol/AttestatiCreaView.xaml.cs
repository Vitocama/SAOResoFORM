using SAOResoForm.Models;
using SAOResoForm.Repositories;
using SAOResoForm.Service.App;
using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SAOResoForm.AttestatiControl.AttestatiCreaControl
{
    public partial class AttestatiCreaView : Window
    {
        public AttestatiCreaView(List<long> personaleIds)
        {
            InitializeComponent();

         

            DataContext = new AttestatiCreaViewModel(personaleIds, new RepositoryAttestato(), new Tool());
        }
    }
}
