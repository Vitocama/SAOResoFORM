using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.Dati;
using SAOResoForm.DBScelta;
using SAOResoForm.Models;
using SAOResoForm.ResourcesDictionary.Dati;
using SAOResoForm.Service.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SAOResoForm.ModificaControl
{
    public class ModificaViewModel : INotifyPropertyChanged
    {
        private readonly IRepositoryService _repository;
        private readonly RepartiSezioniNucleoKeyValue _keyValues = new RepartiSezioniNucleoKeyValue();

        // Personale dal DB - MODIFICATO DIRETTAMENTE
        private Personale _personaleOriginale;

        // ============ VALORI ORIGINALI PER RINOMINA CARTELLA ============
        private string _cognomeOriginale;
        private string _nomeOriginale;
        private string _matricolaOriginale;

        // ============ EVENTO PER NOTIFICARE L'AGGIORNAMENTO ============
        public event EventHandler DatiAggiornati;

        // ============ CONTROLLO VISIBILITÀ LISTBOX ============
        private bool _modificaRepartoSezioneNucleo;
        public bool ModificaRepartoSezioneNucleo
        {
            get => _modificaRepartoSezioneNucleo;
            set
            {
                _modificaRepartoSezioneNucleo = value;
                OnPropertyChanged(nameof(ModificaRepartoSezioneNucleo));
            }
        }

        // Proprietà per binding - MODIFICANO DIRETTAMENTE _personaleOriginale
        public string Nome
        {
            get => _personaleOriginale?.Nome;
            set
            {
                if (_personaleOriginale != null && _personaleOriginale.Nome != value)
                {
                    _personaleOriginale.Nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        public string Cognome
        {
            get => _personaleOriginale?.Cognome;
            set
            {
                if (_personaleOriginale != null && _personaleOriginale.Cognome != value)
                {
                    _personaleOriginale.Cognome = value;
                    OnPropertyChanged(nameof(Cognome));
                }
            }
        }

        public string Matricola
        {
            get => _personaleOriginale?.Matricola;
            set
            {
                if (_personaleOriginale != null && _personaleOriginale.Matricola != value)
                {
                    _personaleOriginale.Matricola = value;
                    OnPropertyChanged(nameof(Matricola));
                }
            }
        }

        public string Annotazioni
        {
            get => _personaleOriginale?.Annotazioni;
            set
            {
                if (_personaleOriginale != null && _personaleOriginale.Annotazioni != value)
                {
                    _personaleOriginale.Annotazioni = value;
                    OnPropertyChanged(nameof(Annotazioni));
                }
            }
        }

        public string CategoriaProfilo
        {
            get => _personaleOriginale?.CategoriaProfilo;
            set
            {
                if (_personaleOriginale != null && _personaleOriginale.CategoriaProfilo != value)
                {
                    _personaleOriginale.CategoriaProfilo = value;
                    OnPropertyChanged(nameof(CategoriaProfilo));
                }
            }
        }

        public string GradoQualifica
        {
            get => _personaleOriginale?.GradoQualifica;
            set
            {
                if (_personaleOriginale != null && _personaleOriginale.GradoQualifica != value)
                {
                    _personaleOriginale.GradoQualifica = value;
                    OnPropertyChanged(nameof(GradoQualifica));
                }
            }
        }

        // ComboBox Reparto/Sezione/Nucleo
        public List<string> Reparti { get; set; }

        private string _repartoSelezionato;
        public string RepartoSelezionato
        {
            get => _repartoSelezionato;
            set
            {
                if (_repartoSelezionato != value)
                {
                    _repartoSelezionato = value;
                    OnPropertyChanged(nameof(RepartoSelezionato));

                    // Aggiorna lista Sezioni
                    Sezioni = RepartiSezioniNucleoKeyValue.Data2.ContainsKey(value)
                        ? RepartiSezioniNucleoKeyValue.Data2[value]
                        : new List<string> { " " };
                    OnPropertyChanged(nameof(Sezioni));

                    // Reset selezione Sezione/Nucleo
                    SezioneSelezionata = Sezioni.FirstOrDefault();
                }
            }
        }

        private List<string> _sezioni;
        public List<string> Sezioni
        {
            get => _sezioni;
            set
            {
                _sezioni = value;
                OnPropertyChanged(nameof(Sezioni));
            }
        }

        private string _sezioneSelezionata;
        public string SezioneSelezionata
        {
            get => _sezioneSelezionata;
            set
            {
                if (_sezioneSelezionata != value)
                {
                    _sezioneSelezionata = value;
                    OnPropertyChanged(nameof(SezioneSelezionata));

                    // Aggiorna lista Nuclei
                    Nuclei = RepartiSezioniNucleoKeyValue.TerzoLivello.ContainsKey(value)
                        ? RepartiSezioniNucleoKeyValue.TerzoLivello[value]
                        : new List<string> { " " };
                    OnPropertyChanged(nameof(Nuclei));

                    NucleoSelezionato = Nuclei.FirstOrDefault();
                }
            }
        }

        private List<string> _nuclei;
        public List<string> Nuclei
        {
            get => _nuclei;
            set
            {
                _nuclei = value;
                OnPropertyChanged(nameof(Nuclei));
            }
        }

        private string _nucleoSelezionato;
        public string NucleoSelezionato
        {
            get => _nucleoSelezionato;
            set
            {
                _nucleoSelezionato = value;
                OnPropertyChanged(nameof(NucleoSelezionato));
            }
        }

        // ComboBox Incarico
        public List<string> Incarichi { get; set; }
        private string _incaricoSelezionato;
        public string IncaricoSelezionato
        {
            get => _incaricoSelezionato;
            set
            {
                if (_incaricoSelezionato != value)
                {
                    _incaricoSelezionato = value;
                    if (_personaleOriginale != null)
                    {
                        _personaleOriginale.Incarico = value;
                    }
                    OnPropertyChanged(nameof(IncaricoSelezionato));
                }
            }
        }

        // ComboBox Servizio
        public List<string> Servizio { get; set; }
        private string _servizioSelezionato;
        public string ServizioSelezionato
        {
            get => _servizioSelezionato;
            set
            {
                if (_servizioSelezionato != value)
                {
                    _servizioSelezionato = value;
                    if (_personaleOriginale != null)
                    {
                        _personaleOriginale.StatoServizio = value;
                    }
                    OnPropertyChanged(nameof(ServizioSelezionato));
                }
            }
        }

        // ComboBox Militari
        public List<string> Militari { get; set; }
        private string _militariSelezionato;
        public string MilitariSelezionato
        {
            get => _militariSelezionato;
            set
            {
                if (_militariSelezionato != value)
                {
                    _militariSelezionato = value;
                    if (_personaleOriginale != null)
                    {
                        _personaleOriginale.MilCiv = value;
                    }
                    OnPropertyChanged(nameof(MilitariSelezionato));
                }
            }
        }

        // Comando salva
        public ICommand UpdateCommand { get; }

        public ModificaViewModel(Personale item, IRepositoryService repository)
        {
            _repository = repository;

            // Carica personale originale dal DB
            _personaleOriginale = _repository.GetById(item.Matricola);

            // ============ SALVA I VALORI ORIGINALI PER LA RINOMINA ============
            _cognomeOriginale = _personaleOriginale?.Cognome;
            _nomeOriginale = _personaleOriginale?.Nome;
            _matricolaOriginale = _personaleOriginale?.Matricola;

            // Inizializza liste per ComboBox
            Reparti = _keyValues.Data1.Keys.ToList();
            Incarichi = new List<string> { " ", "DA", "VDA", "CAPO REPARTO", "CAPO SEZIONE", "CAPO NUCLEO", "CAPO UFFICIO", "CAPO SERVIZIO", "ADDETTO", "CAPO UNITA", "RSPP", "ASPP", "RSAQ", "AMMINISTRATORE" };
            Servizio = new List<string> { " ", "ASPETTATIVA", "CESSATO", "COMANDO", "DISTACCO SINDACALE", "IN SERVIZIO", "LUNGA DEGENZA", "TEMPO ASSEGNATO", "TRASFERITO", "TURNISTA", "IN FORZA", "TEMP. ASSEGN" };
            Militari = new List<string> { " ", "MIL", "CIV" };

            // ============ DEFAULT: NON MODIFICARE REPARTO/SEZIONE/NUCLEO ============
            ModificaRepartoSezioneNucleo = false;

            // ============ ESTRAI DA COD UFFICIO (converti long? -> string) ============
            string codUfficioString = ConvertiCodUfficioInStringa(_personaleOriginale?.CodUfficio);
            string repartoIniziale = EstraiRepartoDaCodUfficio(codUfficioString);
            string sezioneIniziale = EstraiSezioneDaCodUfficio(codUfficioString);
            string nucleoIniziale = EstraiNucleoDaCodUfficio(codUfficioString);

            // Imposta selezioni iniziali (priorità: CodUfficio > CodReparto)
            RepartoSelezionato = repartoIniziale ?? _personaleOriginale?.CodReparto ?? Reparti.FirstOrDefault();

            // Attendi che Sezioni sia popolato
            if (!string.IsNullOrWhiteSpace(sezioneIniziale) && Sezioni != null && Sezioni.Contains(sezioneIniziale))
            {
                SezioneSelezionata = sezioneIniziale;
            }
            else if (!string.IsNullOrWhiteSpace(_personaleOriginale?.CodSezione) && Sezioni != null && Sezioni.Contains(_personaleOriginale.CodSezione))
            {
                SezioneSelezionata = _personaleOriginale.CodSezione;
            }

            // Attendi che Nuclei sia popolato
            if (!string.IsNullOrWhiteSpace(nucleoIniziale) && Nuclei != null && Nuclei.Contains(nucleoIniziale))
            {
                NucleoSelezionato = nucleoIniziale;
            }
            else if (!string.IsNullOrWhiteSpace(_personaleOriginale?.CodNucleo) && Nuclei != null && Nuclei.Contains(_personaleOriginale.CodNucleo))
            {
                NucleoSelezionato = _personaleOriginale.CodNucleo;
            }

            IncaricoSelezionato = _personaleOriginale?.Incarico ?? Incarichi.FirstOrDefault();
            ServizioSelezionato = _personaleOriginale?.StatoServizio ?? Servizio.FirstOrDefault();
            MilitariSelezionato = _personaleOriginale?.MilCiv ?? Militari.FirstOrDefault();

            // Comando salva
            UpdateCommand = new RelayCommand(updateCommand);
        }

        // ============ CONVERSIONE COD UFFICIO (long? -> string chiave dizionario) ============
        private string ConvertiCodUfficioInStringa(long? codUfficioNumerico)
        {
            if (!codUfficioNumerico.HasValue)
                return null;

            var codUUOO = new Cod_UUOO();
            var risultato = codUUOO.reparti.FirstOrDefault(x => x.Value == codUfficioNumerico.Value);
            return risultato.Key != null ? risultato.Key : null;
        }

        // ============ METODI DI ESTRAZIONE DA CODUFFICIO (string) ============
        private string EstraiRepartoDaCodUfficio(string codUfficio)
        {
            if (string.IsNullOrWhiteSpace(codUfficio))
                return null;

            codUfficio = codUfficio.Trim();
            if (codUfficio.Contains(" "))
            {
                string reparto = codUfficio.Split(' ')[0].Trim();
                if (Reparti != null && Reparti.Contains(reparto))
                    return reparto;
            }
            else if (Reparti != null && Reparti.Contains(codUfficio))
            {
                return codUfficio;
            }
            return null;
        }

        private string EstraiSezioneDaCodUfficio(string codUfficio)
        {
            if (string.IsNullOrWhiteSpace(codUfficio))
                return null;

            codUfficio = codUfficio.Trim();
            if (codUfficio.Contains(" "))
            {
                string[] parti = codUfficio.Split(' ');
                if (parti.Length >= 2)
                {
                    return parti[1];
                }
            }
            return null;
        }

        private string EstraiNucleoDaCodUfficio(string codUfficio)
        {
            if (string.IsNullOrWhiteSpace(codUfficio))
                return null;

            codUfficio = codUfficio.Trim();
            if (codUfficio.Contains(" "))
            {
                var parti = codUfficio.Split(' ');
                if (parti.Length >= 3)
                {
                    return parti[2].Trim();
                }
            }
            return null;
        }

        private void updateCommand()
        {
            try
            {
                // ============ SALVA IL VECCHIO NOME PRIMA DELLE MODIFICHE ============
                string vecchioCognome = _cognomeOriginale;
                string vecchioNome = _nomeOriginale;
                string vecchiaMatricola = _matricolaOriginale;

                // Salva Reparto/Sezione/Nucleo solo se checkbox attiva
                if (ModificaRepartoSezioneNucleo)
                {
                    _personaleOriginale.CodReparto = RepartoSelezionato;
                    _personaleOriginale.CodSezione = SezioneSelezionata;
                    _personaleOriginale.CodNucleo = NucleoSelezionato;
                }

                // Salva nel database
                string risultato = _repository.Update(_personaleOriginale);

                if (risultato.Contains("successo"))
                {
                    // ============ AGGIORNA ATTESTATI SE NOME/COGNOME È CAMBIATO ============
                    if (vecchioCognome != _personaleOriginale.Cognome || vecchioNome != _personaleOriginale.Nome)
                    {
                        // CORREZIONE: Aggiunti i parametri mancanti vecchioCognome e vecchioNome
                        AggiornaAttestatiDipendente(
                            _personaleOriginale.Matricola,
                            _personaleOriginale.Cognome,
                            _personaleOriginale.Nome,
                            vecchioCognome,
                            vecchioNome
                        );
                    }
                    else
                    {
                        // Se il nome non è cambiato, rinomina solo la cartella (es. cambio matricola)
                        RinominaCartellaPersonale(vecchioCognome, vecchioNome, vecchiaMatricola);
                    }

                    // Aggiorna i valori originali per future modifiche
                    _cognomeOriginale = _personaleOriginale.Cognome;
                    _nomeOriginale = _personaleOriginale.Nome;
                    _matricolaOriginale = _personaleOriginale.Matricola;

                    MessageBox.Show(risultato,
                                    "Aggiornamento",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    // Notifica aggiornamento
                    DatiAggiornati?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show(risultato,
                                    "Aggiornamento",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'aggiornamento: {ex.Message}",
                               "Errore",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }

        // ============ METODO PER AGGIORNARE ATTESTATI ============
        private void AggiornaAttestatiDipendente(string matricola, string nuovoCognome, string nuovoNome, string vecchioCognome, string vecchioNome)
        {
            try
            {
                using (var db = new tblContext())
                {
                    // Trova tutti gli attestati del dipendente
                    var attestatiDaAggiornare = db.Attestati
                        .Where(a => a.MatricolaDipendente == matricola)
                        .ToList();

                    if (attestatiDaAggiornare.Any())
                    {
                        string vecchiaCartella = $"{vecchioCognome.ToUpper()}_{vecchioNome.ToUpper()}_{matricola}";
                        string nuovaCartella = $"{nuovoCognome.ToUpper()}_{nuovoNome.ToUpper()}_{matricola}";

                        string percorsoBase = SceltaDBViewModel.CaricaCartellaAttestati();
                        string vecchioPercorsoCartella = Path.Combine(percorsoBase, vecchiaCartella);
                        string nuovoPercorsoCartella = Path.Combine(percorsoBase, nuovaCartella);

                        // Rinomina la cartella se esiste
                        if (Directory.Exists(vecchioPercorsoCartella))
                        {
                            if (!Directory.Exists(nuovoPercorsoCartella))
                            {
                                Directory.Move(vecchioPercorsoCartella, nuovoPercorsoCartella);
                            }
                            else
                            {
                                MessageBox.Show($"La cartella {nuovaCartella} esiste già!",
                                               "Attenzione",
                                               MessageBoxButton.OK,
                                               MessageBoxImage.Warning);
                                return;
                            }
                        }

                        // Aggiorna gli attestati nel database
                        foreach (var attestato in attestatiDaAggiornare)
                        {
                            attestato.Dipendente = $"{nuovoCognome} {nuovoNome}";

                            // Aggiorna solo il percorso della cartella, mantenendo il nome del file
                            if (!string.IsNullOrEmpty(attestato.LinkAttestato))
                            {
                                string nomeFile = Path.GetFileName(attestato.LinkAttestato);
                                attestato.LinkAttestato = Path.Combine(nuovoPercorsoCartella, nomeFile);
                            }
                        }

                        db.SaveChanges();

                        MessageBox.Show($"Aggiornati {attestatiDaAggiornare.Count} attestati.\nCartella rinominata da:\n{vecchiaCartella}\na:\n{nuovaCartella}",
                                       "Attestati Aggiornati",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'aggiornamento degli attestati: {ex.Message}",
                               "Errore",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }

        // ============ METODO PER RINOMINARE LA CARTELLA ============
        private void RinominaCartellaPersonale(string vecchioCognome, string vecchioNome, string vecchiaMatricola)
        {
            try
            {
                string cartellaBase =SceltaDBViewModel.CaricaCartellaAttestati();

                string vecchioNomeCartella = $"{vecchioCognome.ToUpper()}_{vecchioNome.ToUpper()}_{vecchiaMatricola}";
                string nuovoNomeCartella = $"{_personaleOriginale.Cognome.ToUpper()}_{_personaleOriginale.Nome.ToUpper()}_{_personaleOriginale.Matricola}";

                string vecchioPercorso = Path.Combine(cartellaBase, vecchioNomeCartella);
                string nuovoPercorso = Path.Combine(cartellaBase, nuovoNomeCartella);

                // Se il nome non è cambiato, non fare nulla
                if (vecchioNomeCartella.Equals(nuovoNomeCartella, StringComparison.OrdinalIgnoreCase))
                    return;

                // Se la vecchia cartella esiste, rinominala
                if (Directory.Exists(vecchioPercorso))
                {
                    if (!Directory.Exists(nuovoPercorso))
                    {
                        Directory.Move(vecchioPercorso, nuovoPercorso);
                        MessageBox.Show($"Cartella rinominata:\n'{vecchioNomeCartella}' → '{nuovoNomeCartella}'",
                                       "Cartella Aggiornata",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"La cartella '{nuovoNomeCartella}' esiste già!",
                                       "Attenzione",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Warning);
                    }
                }
                else
                {
                    // La cartella non esiste: creala con il nuovo nome
                    if (!Directory.Exists(nuovoPercorso))
                    {
                        Directory.CreateDirectory(nuovoPercorso);
                        MessageBox.Show($"Cartella creata: '{nuovoNomeCartella}'",
                                       "Cartella Creata",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Information);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Permessi insufficienti.\nEsegui come amministratore.",
                               "Errore Permessi",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Errore I/O: {ex.Message}",
                               "Errore",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore: {ex.Message}",
                               "Errore",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }

        // PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}