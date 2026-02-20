using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.DBScelta
{
    public class SceltaDBViewModel : INotifyPropertyChanged
    {
        // ============ PERCORSO FILE CONFIGURAZIONE ============
        public static readonly string FileConfigPercorso = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "db_config.txt");

        private string _percorsoDb;
        public string PercorsoDb
        {
            get => _percorsoDb;
            set
            {
                _percorsoDb = value;
                OnPropertyChanged(nameof(PercorsoDb));
            }
        }

        public ICommand SfogliaCommand { get; }
        public ICommand SalvaCommand { get; }

        public SceltaDBViewModel()
        {
            // Carica percorso salvato se esiste
            PercorsoDb = CaricaPercorso();

            SfogliaCommand = new RelayCommand(Sfoglia);
            SalvaCommand = new RelayCommand(Salva, () => !string.IsNullOrWhiteSpace(PercorsoDb));
        }

        private void Sfoglia()
        {
            string initialDir = @"C:\SAO\TBL";

            if (!string.IsNullOrWhiteSpace(PercorsoDb))
            {
                string dir = Path.GetDirectoryName(PercorsoDb);
                if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
                    initialDir = dir;
            }

            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Seleziona il database SQLite",
                Filter = "SQLite Database (*.sqlite)|*.sqlite|Tutti i file (*.*)|*.*",
                InitialDirectory = initialDir
            };

            if (dialog.ShowDialog() == true)
            {
                PercorsoDb = dialog.FileName;
            }
        }

        private void Salva()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PercorsoDb))
                {
                    MessageBox.Show("Seleziona prima un database.",
                                   "Attenzione",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Warning);
                    return;
                }

                if (!File.Exists(PercorsoDb))
                {
                    MessageBox.Show($"Il file non esiste:\n{PercorsoDb}",
                                   "Attenzione",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Warning);
                    return;
                }

                // Crea la cartella se non esiste
                string cartella = Path.GetDirectoryName(FileConfigPercorso);
                if (!Directory.Exists(cartella))
                    Directory.CreateDirectory(cartella);

                File.WriteAllText(FileConfigPercorso, PercorsoDb);
                MessageBox.Show($"Percorso salvato:\n{PercorsoDb}",
                               "Salvataggio OK",
                               MessageBoxButton.OK,
                               MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore nel salvataggio: {ex.Message}\n\nDettaglio: {ex.InnerException?.Message}",
                               "Errore",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }

        // ============ METODO STATICO PER LEGGERE IL PERCORSO ============
        public static string CaricaPercorso()
        {
            try
            {
                if (File.Exists(FileConfigPercorso))
                {
                    string percorso = File.ReadAllText(FileConfigPercorso).Trim();
                    if (!string.IsNullOrWhiteSpace(percorso))
                        return percorso;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}