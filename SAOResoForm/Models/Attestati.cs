using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SAOResoForm.Models
{
    public class Attestati : INotifyPropertyChanged
    {
        #region Fields

        private int _id;
        private bool _attivita;
        private string _matricolaDipendente;
        private string _titoloCorso;
        private string _codiceAttivitaFormativa;
        private string _codiceMateriaCorso;
        private string _enteFormatore;
        private string _denominazioneEnteCertificatore;
        private string _dataInizioCorso;
        private string _dataFineCorso;
        private string _annoCorso;
        private string _validitaAnni;
        private string _dataScadenzaCorso;
        private string _linkAttestato;

        #endregion

        #region Properties

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public bool Attivo
        {
            get => _attivita;
            set => SetProperty(ref _attivita, value);
        }

        public string MatricolaDipendente
        {
            get => _matricolaDipendente;
            set => SetProperty(ref _matricolaDipendente, value);
        }

        public string TitoloCorso
        {
            get => _titoloCorso;
            set => SetProperty(ref _titoloCorso, value);
        }

        public string CodiceAttivitaFormativa
        {
            get => _codiceAttivitaFormativa;
            set => SetProperty(ref _codiceAttivitaFormativa, value);
        }

        public string CodiceMateriaCorso
        {
            get => _codiceMateriaCorso;
            set => SetProperty(ref _codiceMateriaCorso, value);
        }

        public string EnteFormatore
        {
            get => _enteFormatore;
            set => SetProperty(ref _enteFormatore, value);
        }

        public string DenominazioneEnteCertificatore
        {
            get => _denominazioneEnteCertificatore;
            set => SetProperty(ref _denominazioneEnteCertificatore, value);
        }

        public string DataInizioCorso
        {
            get => _dataInizioCorso;
            set => SetProperty(ref _dataInizioCorso, value);
        }

        public string DataFineCorso
        {
            get => _dataFineCorso;
            set => SetProperty(ref _dataFineCorso, value);
        }

        public string AnnoCorso
        {
            get => _annoCorso;
            set => SetProperty(ref _annoCorso, value);
        }

        public string ValiditaAnni
        {
            get => _validitaAnni;
            set => SetProperty(ref _validitaAnni, value);
        }

        public string DataScadenzaCorso
        {
            get => _dataScadenzaCorso;
            set => SetProperty(ref _dataScadenzaCorso, value);
        }

        public string LinkAttestato
        {
            get => _linkAttestato;
            set => SetProperty(ref _linkAttestato, value);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}