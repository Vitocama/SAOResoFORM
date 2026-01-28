using GalaSoft.MvvmLight.CommandWpf;
using SAOResoForm.Dati;
using SAOResoForm.Models;
using SAOResoForm.ResourcesDictionary.Dati;
using SAOResoForm.Service.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace SAOResoForm.ModificaControl
{
    internal class ModificaViewModel : INotifyPropertyChanged
    {
        private readonly IRepositoryService _repository;

        // Istanza dati Reparto/Sezione/Nucleo
        private readonly RepartiSezioniNucleoKeyValue _keyValues = new RepartiSezioniNucleoKeyValue();

        // Personale da modificare
        private Personale _personale;
        public Personale Personale
        {
            get => _personale;
            set
            {
                _personale = value;
                OnPropertyChanged(nameof(Personale));
                OnPropertyChanged(nameof(Nome));
                OnPropertyChanged(nameof(Cognome));
                OnPropertyChanged(nameof(Matricola));
                OnPropertyChanged(nameof(Annotazioni));
                OnPropertyChanged(nameof(CategoriaProfilo));
                OnPropertyChanged(nameof(GradoQualifica));
            }
        }

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

        // Proprietà per binding dati anagrafici
        public string Nome
        {
            get => Personale?.Nome;
            set
            {
                if (Personale != null)
                {
                    Personale.Nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        public string Cognome
        {
            get => Personale?.Cognome;
            set
            {
                if (Personale != null)
                {
                    Personale.Cognome = value;
                    OnPropertyChanged(nameof(Cognome));
                }
            }
        }

        public string Matricola
        {
            get => Personale?.Matricola;
            set
            {
                if (Personale != null)
                {
                    Personale.Matricola = value;
                    OnPropertyChanged(nameof(Matricola));
                }
            }
        }

        public string Annotazioni
        {
            get => Personale?.Annotazioni;
            set
            {
                if (Personale != null)
                {
                    Personale.Annotazioni = value;
                    OnPropertyChanged(nameof(Annotazioni));
                }
            }
        }

        public string CategoriaProfilo
        {
            get => Personale?.CategoriaProfilo;
            set
            {
                if (Personale != null)
                {
                    Personale.CategoriaProfilo = value;
                    OnPropertyChanged(nameof(CategoriaProfilo));
                }
            }
        }

        public string GradoQualifica
        {
            get => Personale?.GradoQualifica;
            set
            {
                if (Personale != null)
                {
                    Personale.GradoQualifica = value;
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
            set { _sezioni = value; OnPropertyChanged(nameof(Sezioni)); }
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
            set { _nuclei = value; OnPropertyChanged(nameof(Nuclei)); }
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
                _incaricoSelezionato = value;

                // Aggiorna il dato nel modello
                if (Personale != null)
                    Personale.Incarico = value;

                OnPropertyChanged(nameof(IncaricoSelezionato));
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
                _servizioSelezionato = value;

                // Aggiorna il dato nel modello
                if (Personale != null)
                    Personale.StatoServizio = value;

                OnPropertyChanged(nameof(ServizioSelezionato));
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
                _militariSelezionato = value;

                // Aggiorna il dato nel modello
                if (Personale != null)
                    Personale.MilCiv = value;

                OnPropertyChanged(nameof(MilitariSelezionato));
            }
        }

        // Comando salva
        public ICommand SalvaCommand { get; }

        public ModificaViewModel(Personale item, IRepositoryService repository)
        {
            _repository = repository;

            // Carica dati dal DB
            Personale = _repository.GetById(item.Matricola);

            // Inizializza liste per ComboBox
            Reparti = _keyValues.Data1.Keys.ToList();

            Incarichi = new List<string>
            {
                " ", "DA", "VDA", "CAPO REPARTO", "CAPO SEZIONE", "CAPO NUCLEO",
                "CAPO UFFICIO", "CAPO SERVIZIO", "ADDETTO", "CAPO UNITA",
                "RSPP", "ASPP", "RSAQ", "AMMINISTRATORE"
            };

            Servizio = new List<string>
            {
                " ", "ASPETTATIVA", "CESSATO", "COMANDO", "DISTACCO SINDACALE",
                "IN SERVIZIO", "LUNGA DEGENZA", "TEMPO ASSEGNATO", "TRASFERITO",
                "TURNISTA", "IN FORZA", "TEMP. ASSEGN"
            };

            Militari = new List<string>
            {
                " ", "MIL", "CIV"
            };

            // ============ DEFAULT: NON MODIFICARE REPARTO/SEZIONE/NUCLEO ============
            ModificaRepartoSezioneNucleo = false;

            // ============ ESTRAI DA COD UFFICIO (converti long? -> string) ============
            string codUfficioString = ConvertiCodUfficioInStringa(Personale?.CodUfficio);
            string repartoIniziale = EstraiRepartoDaCodUfficio(codUfficioString);
            string sezioneIniziale = EstraiSezioneDaCodUfficio(codUfficioString);
            string nucleoIniziale = EstraiNucleoDaCodUfficio(codUfficioString);

            // Imposta selezioni iniziali (priorità: CodUfficio > CodReparto)
            RepartoSelezionato = repartoIniziale
                ?? Personale?.CodReparto
                ?? Reparti.FirstOrDefault();

            // Attendi che Sezioni sia popolato
            if (!string.IsNullOrWhiteSpace(sezioneIniziale) &&
                Sezioni != null && Sezioni.Contains(sezioneIniziale))
            {
                SezioneSelezionata = sezioneIniziale;
            }
            else if (!string.IsNullOrWhiteSpace(Personale?.CodSezione) &&
                     Sezioni != null && Sezioni.Contains(Personale.CodSezione))
            {
                SezioneSelezionata = Personale.CodSezione;
            }

            // Attendi che Nuclei sia popolato
            if (!string.IsNullOrWhiteSpace(nucleoIniziale) &&
                Nuclei != null && Nuclei.Contains(nucleoIniziale))
            {
                NucleoSelezionato = nucleoIniziale;
            }
            else if (!string.IsNullOrWhiteSpace(Personale?.CodNucleo) &&
                     Nuclei != null && Nuclei.Contains(Personale.CodNucleo))
            {
                NucleoSelezionato = Personale.CodNucleo;
            }

            IncaricoSelezionato = Personale?.Incarico ?? Incarichi.FirstOrDefault();
            ServizioSelezionato = Personale?.StatoServizio ?? Servizio.FirstOrDefault();
            MilitariSelezionato = Personale?.MilCiv ?? Militari.FirstOrDefault();

            // Comando salva
            SalvaCommand = new RelayCommand(Salva);
        }

        // ============ CONVERSIONE COD UFFICIO (long? -> string chiave dizionario) ============

        /// <summary>
        /// Converte il CodUfficio numerico nella chiave stringa del dizionario Cod_UUOO
        /// Es: 120 -> "DA - SPP"
        /// </summary>
        private string ConvertiCodUfficioInStringa(long? codUfficioNumerico)
        {
            if (!codUfficioNumerico.HasValue)
                return null;

            var codUUOO = new Cod_UUOO();

            // Cerca la chiave corrispondente al valore numerico
            var risultato = codUUOO.reparti.FirstOrDefault(x => x.Value == codUfficioNumerico.Value);

            // Se trovato, ritorna la chiave (es: "DA - SPP"), altrimenti null
            return risultato.Key != null ? risultato.Key : null;
        }

        // ============ METODI DI ESTRAZIONE DA CODUFFICIO (string) ============

        /// <summary>
        /// Estrae il reparto dal CodUfficio (primo elemento prima del -)
        /// Es: "DA - SPP" → "DA"
        /// </summary>
        private string EstraiRepartoDaCodUfficio(string codUfficio)
        {
            if (string.IsNullOrWhiteSpace(codUfficio))
                return null;

            codUfficio = codUfficio.Trim();

            if (codUfficio.Contains("-"))
            {
                string reparto = codUfficio.Split('-')[0].Trim();
                if (Reparti != null && Reparti.Contains(reparto))
                    return reparto;
            }
            else if (Reparti != null && Reparti.Contains(codUfficio))
            {
                return codUfficio;
            }

            return null;
        }

        /// <summary>
        /// Estrae la sezione dal CodUfficio (secondo elemento)
        /// Es: "DA - SPP - AMBIENTE" → "SPP"
        /// </summary>
        private string EstraiSezioneDaCodUfficio(string codUfficio)
        {
            if (string.IsNullOrWhiteSpace(codUfficio))
                return null;

            codUfficio = codUfficio.Trim();

            if (codUfficio.Contains("-"))
            {
                var parti = codUfficio.Split('-');
                if (parti.Length >= 2)
                {
                    return parti[1].Trim();
                }
            }

            return null;
        }

        /// <summary>
        /// Estrae il nucleo dal CodUfficio (terzo elemento)
        /// Es: "DA - SPP - AMBIENTE" → "AMBIENTE"
        /// </summary>
        private string EstraiNucleoDaCodUfficio(string codUfficio)
        {
            if (string.IsNullOrWhiteSpace(codUfficio))
                return null;

            codUfficio = codUfficio.Trim();

            if (codUfficio.Contains("-"))
            {
                var parti = codUfficio.Split('-');
                if (parti.Length >= 3)
                {
                    return parti[2].Trim();
                }
            }

            return null;
        }

        private void Salva()
        {
            // ============ SALVA SOLO SE CHECKBOX È ATTIVO ============
            if (ModificaRepartoSezioneNucleo)
            {
                Personale.CodReparto = RepartoSelezionato;
                Personale.CodSezione = SezioneSelezionata;
                Personale.CodNucleo = NucleoSelezionato;
            }
            // Altrimenti Reparto/Sezione/Nucleo rimangono invariati

            _repository.Update(Personale);
        }

        // PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}