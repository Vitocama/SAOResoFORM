using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

// RIMOSSO: using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace SAOResoForm.DBScelta
{
    public class SceltaDBViewModel : INotifyPropertyChanged
    {
        public static readonly string FileConfigPercorso = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "db_config.txt");

        public static readonly string FileConfigAttestati = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "attestati.txt");

        private string _percorsoDb;
        public string PercorsoDb
        {
            get => _percorsoDb;
            set { _percorsoDb = value; OnPropertyChanged(nameof(PercorsoDb)); }
        }

        private string _cartellaAttestati;
        public string CartellaAttestati
        {
            get => _cartellaAttestati;
            set { _cartellaAttestati = value; OnPropertyChanged(nameof(CartellaAttestati)); }
        }

        public ICommand SfogliaCommand { get; }
        public ICommand SfogliaAttestatiCommand { get; }
        public ICommand SalvaCommand { get; }

        public SceltaDBViewModel()
        {
            PercorsoDb = CaricaPercorso();
            CartellaAttestati = CaricaCartellaAttestati();

            SfogliaCommand = new RelayCommand(Sfoglia);
            SfogliaAttestatiCommand = new RelayCommand(SfogliaAttestati);
            SalvaCommand = new RelayCommand(Salva, () =>
                !string.IsNullOrWhiteSpace(PercorsoDb) &&
                !string.IsNullOrWhiteSpace(CartellaAttestati));
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

            // Microsoft.Win32.OpenFileDialog → ShowDialog() restituisce bool?
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Seleziona il database SQLite",
                Filter = "SQLite Database (*.sqlite)|*.sqlite|Tutti i file (*.*)|*.*",
                InitialDirectory = initialDir
            };

            if (dialog.ShowDialog() == true)
                PercorsoDb = dialog.FileName;
        }

        private void SfogliaAttestati()
        {
            // System.Windows.Forms.FolderBrowserDialog → ShowDialog() restituisce DialogResult
            using (var dialog = new FolderBrowserDialog
            {
                Description = "Seleziona la cartella degli attestati",
                ShowNewFolderButton = true,
                SelectedPath = Directory.Exists(CartellaAttestati) ? CartellaAttestati : string.Empty
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    CartellaAttestati = dialog.SelectedPath;
            }
        }

        private void Salva()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PercorsoDb))
                {
                    MessageBox.Show("Seleziona prima un database.",
                        "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!File.Exists(PercorsoDb))
                {
                    MessageBox.Show($"Il file non esiste:\n{PercorsoDb}",
                        "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(CartellaAttestati))
                {
                    MessageBox.Show("Seleziona prima la cartella degli attestati.",
                        "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                File.WriteAllText(FileConfigPercorso, PercorsoDb);
                File.WriteAllText(FileConfigAttestati, CartellaAttestati);

                MessageBox.Show(
                    $"Configurazione salvata:\n\nDatabase: {PercorsoDb}\nAttestati: {CartellaAttestati}",
                    "Salvataggio OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Errore nel salvataggio: {ex.Message}\n\nDettaglio: {ex.InnerException?.Message}",
                    "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string CaricaPercorso()
        {
            try
            {
                if (File.Exists(FileConfigPercorso))
                {
                    string percorso = File.ReadAllText(FileConfigPercorso).Trim();
                    if (!string.IsNullOrWhiteSpace(percorso)) return percorso;
                }
                return string.Empty;
            }
            catch { return string.Empty; }
        }

        public static string CaricaCartellaAttestati()
        {
            try
            {
                if (File.Exists(FileConfigAttestati))
                {
                    string cartella = File.ReadAllText(FileConfigAttestati).Trim();
                    if (!string.IsNullOrWhiteSpace(cartella)) return cartella;
                }
                return string.Empty;
            }
            catch { return string.Empty; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}