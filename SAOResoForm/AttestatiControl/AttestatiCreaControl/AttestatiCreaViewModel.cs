using RESOFORM.Dati;
using SAOResoForm.DBScelta;
using SAOResoForm.Models;
using SAOResoForm.Repositories;
using SAOResoForm.Service.Repository.tool;
using SAOResoForm.informazioneControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SAOResoForm.AttestatiControl.AttestatiCreaControl
{
    public class AttestatiCreaViewModel : INotifyPropertyChanged
    {
        private readonly IRepositoryAttestato _repositoryAttestato;
        private readonly ITool _tool;

        public event PropertyChangedEventHandler PropertyChanged;

        // Flag per controllare se è già stato salvato
        private bool _isSalvato = false;

        #region proprieta
        // =======================
        // PROPRIETÀ DEL FORM
        // =======================
        public ObservableCollection<Personale> PersonaleSelezionato { get; set; }
        public ObservableCollection<string> AttivitaFormativeList { get; set; } = new ObservableCollection<string>();

        private string _attivitaFormativaSelezionata;
        public string AttivitaFormativaSelezionata
        {
            get => _attivitaFormativaSelezionata;
            set { _attivitaFormativaSelezionata = value; OnPropertyChanged(nameof(AttivitaFormativaSelezionata)); }
        }

        private string _titoloCorso;
        public string TitoloCorso
        {
            get => _titoloCorso;
            set { _titoloCorso = value; OnPropertyChanged(nameof(TitoloCorso)); }
        }

        private string _enteFormatore;
        public string EnteFormatore
        {
            get => _enteFormatore;
            set { _enteFormatore = value; OnPropertyChanged(nameof(EnteFormatore)); }
        }

        private string _enteCertificatore;
        public string EnteCertificatore
        {
            get => _enteCertificatore;
            set { _enteCertificatore = value; OnPropertyChanged(nameof(EnteCertificatore)); }
        }

        private string _materia;
        public string Materia
        {
            get => _materia;
            set { _materia = value; OnPropertyChanged(nameof(Materia)); }
        }

        private DateTime? _dataInizio;
        public DateTime? DataInizio
        {
            get => _dataInizio;
            set { _dataInizio = value; OnPropertyChanged(nameof(DataInizio)); }
        }

        private DateTime? _dataFine;
        public DateTime? DataFine
        {
            get => _dataFine;
            set { _dataFine = value; OnPropertyChanged(nameof(DataFine)); }
        }

        private string _validitaAnni;
        public string ValiditaAnni
        {
            get => _validitaAnni;
            set { _validitaAnni = value; OnPropertyChanged(nameof(ValiditaAnni)); }
        }

        private string _nomeFile;
        public string NomeFile
        {
            get => _nomeFile;
            set { _nomeFile = value; OnPropertyChanged(nameof(NomeFile)); }
        }

        private string _percorsoFile;
        public string PercorsoFile
        {
            get => _percorsoFile;
            set { _percorsoFile = value; OnPropertyChanged(nameof(PercorsoFile)); }
        }

        private string _messaggioValidazione;
        public string MessaggioValidazione
        {
            get => _messaggioValidazione;
            set { _messaggioValidazione = value; OnPropertyChanged(nameof(MessaggioValidazione)); }
        }
        #endregion

        public string PersonaleInfo => PersonaleSelezionato != null && PersonaleSelezionato.Count > 0
            ? string.Join(", ", PersonaleSelezionato.Select(p => $"{p.Nome} {p.Cognome}"))
            : "Nessun personale selezionato";

        // =======================
        // COMANDI
        // =======================
        public ICommand SalvaCommand { get; set; }
        public ICommand CaricaFileCommand { get; set; }
        public ICommand RimuoviFileCommand { get; set; }
        public ICommand AnnullaCommand { get; set; }

        // =======================
        // COSTRUTTORE
        // =======================
        public AttestatiCreaViewModel(List<long> personaleIds, IRepositoryAttestato repositoryAttestato, ITool tool)
        {
            _repositoryAttestato = repositoryAttestato ?? throw new ArgumentNullException(nameof(repositoryAttestato));
            _tool = tool ?? throw new ArgumentNullException(nameof(tool));

            PersonaleSelezionato = new ObservableCollection<Personale>(GetPersonaleByIds(personaleIds));

            // Inizializza comandi con CanExecute per il comando Salva
            SalvaCommand = new RelayCommand(
                async _ => await SalvaAttestatiAsync(),
                _ => !_isSalvato  // Disabilita se già salvato
            );
            CaricaFileCommand = new RelayCommand(_ => CaricaFile());
            RimuoviFileCommand = new RelayCommand(_ => RimuoviFile());
            AnnullaCommand = new RelayCommand(param => { if (param is Window w) w.Close(); });

            // Popola attività formative
            AttivitaFormativaComboBox combo = new AttivitaFormativaComboBox();
            AttivitaFormativeList = new ObservableCollection<string>(combo.formativa);
        }

        // =======================
        // METODI PRIVATI
        // =======================
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(PersonaleSelezionato))
                OnPropertyChanged(nameof(PersonaleInfo));
        }

        private List<Personale> GetPersonaleByIds(List<long> ids)
        {
            var context = new tblContext();
            return context.Personale.Where(p => ids.Contains(p.Id)).ToList();
        }

        private async Task SalvaAttestatiAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(PercorsoFile) || !File.Exists(PercorsoFile))
                {
                    MessaggioValidazione = "Selezionare un file valido.";
                    return;
                }

                // Valida ValiditaAnni
                if (string.IsNullOrWhiteSpace(ValiditaAnni))
                {
                    MessaggioValidazione = "Il campo Validità Anni è obbligatorio.";
                    return;
                }

                if (!int.TryParse(ValiditaAnni, out int validitaAnniInt))
                {
                    MessaggioValidazione = "Validità anni deve essere un numero intero.";
                    return;
                }

                if (validitaAnniInt < 0)
                {
                    MessaggioValidazione = "Validità anni non può essere negativo.";
                    return;
                }

                string cartellaBase = SceltaDBViewModel.CaricaCartellaAttestati();
                Directory.CreateDirectory(cartellaBase);

                var db = new tblContext();

                int nuovoId = db.Attestati.Any() ? db.Attestati.Max(a => a.Id) + 1 : 1;

                // ============ GENERA IL NUMERO RANDOM UNA SOLA VOLTA ============
                Random random = new Random();
                int numeroRandomComune = random.Next(1000, 9999);

                foreach (var personale in PersonaleSelezionato)
                {
                    var matricola = personale.Matricola.Split('/').ToArray();
                    string nomeCartellaPersonale = $"{personale.Cognome.Trim()}_{personale.Nome.Trim()}_{matricola[0].Trim()}";
                    string percorsoCartellaPersonale = Path.Combine(cartellaBase, nomeCartellaPersonale);
                    Directory.CreateDirectory(percorsoCartellaPersonale);

                    // ============ USA IL NUMERO RANDOM COMUNE ============
                    string percorsoNuovoFile = _tool.RinominaFile(
                        personale,
                        DataFine ?? DateTime.Now,
                        PercorsoFile,
                        percorsoCartellaPersonale,
                        numeroRandomComune
                    );

                    var reparto = db.Personale.Where(x => x.Matricola == personale.Matricola).ToList().FirstOrDefault()?.CodReparto ?? "REPARTO_NON_DEFINITO";
                    var sezione = db.Personale.Where(x => x.Matricola == personale.Matricola).ToList().FirstOrDefault()?.CodSezione ?? "SEZIONE_NON_DEFINITO";
                    var nucleo = db.Personale.Where(x => x.Matricola == personale.Matricola).ToList().FirstOrDefault()?.CodNucleo ?? "NUCLEO_NON_DEFINITO";

                    var attestato = new Attestati
                    {
                        Id = nuovoId,
                        Dipendente = $"{personale.Cognome} {personale.Nome}".ToUpper(),
                        MatricolaDipendente = personale.Matricola,
                        CodiceAttivitaFormativa = AttivitaFormativaSelezionata.ToUpper(),
                        CodiceMateriaCorso = Materia.ToUpper(),
                        EnteFormatore = EnteFormatore.ToUpper(),
                        DenominazioneEnteCertificatore = EnteCertificatore.ToUpper(),
                        DataFineCorso = DataFine?.ToString("dd-MM-yyyy"),
                        DataInizioCorso = DataInizio?.ToString("dd-MM-yyyy"),
                        ValiditaAnni = validitaAnniInt.ToString(),
                        TitoloCorso = TitoloCorso.ToUpper(),
                        DataScadenzaCorso = DataFine.HasValue && validitaAnniInt > 0
                            ? DataFine.Value.AddYears(validitaAnniInt).ToString("dd-MM-yyyy")
                            : validitaAnniInt == 0
                                ? "01-01-3000"
                                : null,
                        AnnoCorso = DataFine?.Year.ToString(),
                        LinkAttestato = percorsoNuovoFile,
                        Reparto = reparto,
                        Sezione = sezione,
                        Nucleo = nucleo
                    };

                    db.Attestati.Add(attestato);
                    nuovoId++;

                    // ============ GESTIONE CONTATORE BACKUP ============
                    int salvataggi = int.Parse(File.ReadAllText("count.txt"));
                    if (salvataggi >= 20)
                    {
                        EseguiBackup();
                        salvataggi = 0;
                        File.WriteAllText("count.txt", salvataggi.ToString());
                    }
                    else
                    {
                        salvataggi = salvataggi + 1;
                        File.WriteAllText("count.txt", salvataggi.ToString());
                    }
                }

                await db.SaveChangesAsync();

                _isSalvato = true;
                (SalvaCommand as RelayCommand)?.RaiseCanExecuteChanged();

                MessaggioValidazione = $"Attestati salvati con successo! Totale: {PersonaleSelezionato.Count}";
            }
            catch (Exception ex)
            {
                MessaggioValidazione = $"Errore durante il salvataggio: {ex.Message}";
            }
        }

        // ============ BACKUP ============
        private void EseguiBackup()
        {
            string dest = @"C:\SAO";
            string fileDest = File.ReadAllText("db_config.txt").Trim();
            string attestati = File.ReadAllText("attestati.txt").Trim();
            string attestatiBackup = Path.Combine(dest, "SAO");
            string dbFileName = Path.GetFileName(fileDest);
            string dbDest = Path.Combine(dest, dbFileName);

            try
            {
                if (!Directory.Exists(dest))
                    Directory.CreateDirectory(dest);

                File.Copy(fileDest, dbDest, overwrite: true);
                CopiaCartella(attestati, attestatiBackup);

                MessageBox.Show(
                    $"Backup eseguito con successo!\nDB salvato in: {dbDest}\nAttestati salvati in: {attestatiBackup}",
                    "Backup Riuscito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Permessi insufficienti per accedere alla cartella di backup.",
                    "Backup Fallito", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"File sorgente non trovato:\n{fileDest}",
                    "Backup Fallito", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante il backup: {ex.Message}",
                    "Backup Fallito", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopiaCartella(string sorgente, string destinazione)
        {
            if (!Directory.Exists(destinazione))
                Directory.CreateDirectory(destinazione);

            foreach (string file in Directory.GetFiles(sorgente))
            {
                string nomeFile = Path.GetFileName(file);
                string destFile = Path.Combine(destinazione, nomeFile);
                File.Copy(file, destFile, overwrite: true);
            }

            foreach (string sottocartella in Directory.GetDirectories(sorgente))
            {
                string nomeSottocartella = Path.GetFileName(sottocartella);
                string destSottocartella = Path.Combine(destinazione, nomeSottocartella);
                CopiaCartella(sottocartella, destSottocartella);
            }
        }

        // ============ METODO PER GENERARE NOME FILE UNICO ============
        private string GeneraNomeFileUnico(DateTime dataFine, string percorsoOriginale)
        {
            string estensione = Path.GetExtension(percorsoOriginale);
            Random random = new Random();
            int randomNum = random.Next(1000, 9999);
            string dataFormattata = dataFine.ToString("dd-MM-yyyy");

            return $"{dataFormattata}_{randomNum}{estensione}";
        }

        private void CaricaFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
                Title = "Seleziona un file"
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                PercorsoFile = openFileDialog.FileName;
                NomeFile = Path.GetFileName(PercorsoFile);
            }
        }

        private void RimuoviFile()
        {
            NomeFile = string.Empty;
            PercorsoFile = string.Empty;
        }
    }

    // =======================
    // VALIDATION RULE
    // =======================
    public class NumericValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return new ValidationResult(false, "Campo obbligatorio");

            if (!int.TryParse(value.ToString(), out int numero))
                return new ValidationResult(false, "Inserire solo numeri interi");

            if (numero < 0)
                return new ValidationResult(false, "Non sono ammessi numeri negativi");

            return ValidationResult.ValidResult;
        }
    }
}