<<<<<<< HEAD
﻿using SAOResoForm.AttestratiCreaControl;
using SAOResoForm.Models;
=======
﻿using SAOResoForm.AttestatiControl.AttestatiCreaControl;
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887
using SAOResoForm.Service.App;
using System;
using System.Windows;

namespace SAOResoForm.AttestatiControl.AttestazioniInserimentoControl
{
    public partial class AttestatiInserimentoView : Window
    {
        private readonly AttestatiInserimentoViewModel _viewModel;

        public AttestatiInserimentoView(MainViewModel mainVM, AppServices appServices)
        {
            InitializeComponent();

            _viewModel = new AttestatiInserimentoViewModel(mainVM, appServices);
            DataContext = _viewModel;
<<<<<<< HEAD

            _viewModel.RichiediAperturaCreaAttestato += OnRichiediAperturaCreaAttestato;
        }

        private void OnRichiediAperturaCreaAttestato(object sender, Personale personale)
        {
            var vm = new AttestatiCreaViewModel(personale, _viewModel.AppServices);
            var view = new AttestatiCreaView(vm);

            view.ShowDialog();
        }

=======
        }

        // DOPPIO CLICK SUL DATAGRID
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel?.PersonaleSelezionato == null)
                return;

            // ✅ APRI SOLO LA VIEW GIUSTA
            var creaView = new AttestatiCreaView
            {
                Owner = this,
                DataContext = new AttestatiCreaViewModel(
                    _viewModel.PersonaleSelezionato, // ✔ Personale corretto
                    _viewModel.AppServices           // ✔ AppServices già esistente
                )
            };

            creaView.ShowDialog();

            // refresh dati
            _viewModel.AggiornaDati();
        }
>>>>>>> 305da8a0420ff716e2d789a230478eb1b16c1887

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.RichiediAperturaCreaAttestato -= OnRichiediAperturaCreaAttestato;
            _viewModel.Dispose();
            base.OnClosed(e);
        }
    }
}
