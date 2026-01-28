using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAOResoForm.Models
{
    public partial class tblContext : DbContext
    {
        public tblContext()
        {
        }

        public tblContext(DbContextOptions<tblContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountUtenti> AccountUtenti { get; set; }
        public virtual DbSet<Attestati> Attestati { get; set; }
        public virtual DbSet<AttestatiVuota> AttestatiVuota { get; set; }
        public virtual DbSet<CatMilitari> CatMilitari { get; set; }
        public virtual DbSet<CatalogoTrio> CatalogoTrio { get; set; }
        public virtual DbSet<Corsi> Corsi { get; set; }
        public virtual DbSet<Corso0001> Corso0001 { get; set; }
        public virtual DbSet<EntiFormatori> EntiFormatori { get; set; }
        public virtual DbSet<GradoMilitari> GradoMilitari { get; set; }
        public virtual DbSet<Incarico> Incarico { get; set; }
        public virtual DbSet<MateriaCorso> MateriaCorso { get; set; }
        public virtual DbSet<Param> Param { get; set; }
        public virtual DbSet<Personale> Personale { get; set; }
        public virtual DbSet<ProfiloCivili> ProfiloCivili { get; set; }
        public virtual DbSet<QualificaCivili> QualificaCivili { get; set; }
        public virtual DbSet<StatoServizio> StatoServizio { get; set; }
        public virtual DbSet<Tables> Tables { get; set; }
        public virtual DbSet<TipoFormazione> TipoFormazione { get; set; }
        public virtual DbSet<TipologiaCorso> TipologiaCorso { get; set; }
        public virtual DbSet<To> To { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=C:\\SAO\\TBL\\tbl.sqlite");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountUtenti>(entity =>
            {
                entity.ToTable("ACCOUNT_UTENTI");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amministratore).HasColumnName("AMMINISTRATORE");

                entity.Property(e => e.Cognome).HasColumnName("COGNOME");

                entity.Property(e => e.Consultazione).HasColumnName("CONSULTAZIONE");

                entity.Property(e => e.Incarico).HasColumnName("INCARICO");

                entity.Property(e => e.Matricola).HasColumnName("matricola");

                entity.Property(e => e.Nome).HasColumnName("NOME");

                entity.Property(e => e.Password).HasColumnName("PASSWORD");

                entity.Property(e => e.Ruolo).HasColumnName("RUOLO");

                entity.Property(e => e.Utente).HasColumnName("UTENTE");

                entity.Property(e => e.Uuoo).HasColumnName("UUOO");
            });

            modelBuilder.Entity<Attestati>(entity =>
            {
                entity.ToTable("ATTESTATI");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AnnoCorso).HasColumnName("ANNO_CORSO");

                entity.Property(e => e.CodiceAttivitaFormativa).HasColumnName("CODICE_ATTIVITA_FORMATIVA");

                entity.Property(e => e.CodiceMateriaCorso).HasColumnName("CODICE_MATERIA_CORSO");

                entity.Property(e => e.CodiceTipologiaCorso).HasColumnName("CODICE_TIPOLOGIA_CORSO");

                entity.Property(e => e.DataFineCorso).HasColumnName("DATA_FINE_CORSO");

                entity.Property(e => e.DataInizioCorso).HasColumnName("DATA_INIZIO_CORSO");

                entity.Property(e => e.DataNormalizzata).HasColumnName("DATA_NORMALIZZATA");

                entity.Property(e => e.DataScadenzaCorso).HasColumnName("DATA_SCADENZA_CORSO");

                entity.Property(e => e.DenominazioneEnteCertificatore).HasColumnName("DENOMINAZIONE_ENTE_CERTIFICATORE");

                entity.Property(e => e.DenominazioneEnteFormatore).HasColumnName("DENOMINAZIONE_ENTE_FORMATORE");

                entity.Property(e => e.EnteFormatore).HasColumnName("ENTE_FORMATORE");

                entity.Property(e => e.LinkAttestato).HasColumnName("LINK_ATTESTATO");

                entity.Property(e => e.MatricolaDipendente).HasColumnName("MATRICOLA_DIPENDENTE");

                entity.Property(e => e.TitoloCorso).HasColumnName("TITOLO_CORSO");

                entity.Property(e => e.ValiditaAnni).HasColumnName("VALIDITA_ANNI");

                entity.Property(e => e.Variabile)
                    .HasColumnName("VARIABILE")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<AttestatiVuota>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ATTESTATI_VUOTA");

                entity.Property(e => e.AnnoCorso).HasColumnName("ANNO_CORSO");

                entity.Property(e => e.CodiceAttivitaFormativa).HasColumnName("CODICE_ATTIVITA_FORMATIVA");

                entity.Property(e => e.CodiceMateriaCorso).HasColumnName("CODICE_MATERIA_CORSO");

                entity.Property(e => e.CodiceTipologiaCorso).HasColumnName("CODICE_TIPOLOGIA_CORSO");

                entity.Property(e => e.DataFineCorso).HasColumnName("DATA_FINE_CORSO");

                entity.Property(e => e.DataInizioCorso).HasColumnName("DATA_INIZIO_CORSO");

                entity.Property(e => e.DataNormalizzata).HasColumnName("DATA_NORMALIZZATA");

                entity.Property(e => e.DataScadenzaCorso).HasColumnName("DATA_SCADENZA_CORSO");

                entity.Property(e => e.DenominazioneEnteCertificatore).HasColumnName("DENOMINAZIONE_ENTE_CERTIFICATORE");

                entity.Property(e => e.DenominazioneEnteFormatore).HasColumnName("DENOMINAZIONE_ENTE_FORMATORE");

                entity.Property(e => e.EnteFormatore).HasColumnName("ENTE_FORMATORE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LinkAttestato).HasColumnName("LINK_ATTESTATO");

                entity.Property(e => e.MatricolaDipendente).HasColumnName("MATRICOLA_DIPENDENTE");

                entity.Property(e => e.TitoloCorso).HasColumnName("TITOLO_CORSO");

                entity.Property(e => e.ValiditaAnni).HasColumnName("VALIDITA_ANNI");
            });

            modelBuilder.Entity<CatMilitari>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CAT_MILITARI");

                entity.Property(e => e.Area).HasColumnName("AREA");

                entity.Property(e => e.Categoria).HasColumnName("CATEGORIA");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<CatalogoTrio>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CATALOGO_TRIO");

                entity.Property(e => e.AreaTematica).HasColumnName("AREA_TEMATICA");

                entity.Property(e => e.Codice).HasColumnName("CODICE");

                entity.Property(e => e.CodiceAttivitaFormativa).HasColumnName("CODICE_ATTIVITA_FORMATIVA");

                entity.Property(e => e.CodiceMateriaCorso).HasColumnName("CODICE_MATERIA_CORSO");

                entity.Property(e => e.CodiceTestoBreve).HasColumnName("CODICE_TESTO_BREVE");

                entity.Property(e => e.Durata).HasColumnName("DURATA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MacroareaTematica).HasColumnName("MACROAREA_TEMATICA");

                entity.Property(e => e.Titolo).HasColumnName("TITOLO");
            });

            modelBuilder.Entity<Corsi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CORSI");

                entity.Property(e => e.DataFineCorso).HasColumnName("DATA_FINE_CORSO");

                entity.Property(e => e.DataInizioCorso).HasColumnName("DATA_INIZIO_CORSO");

                entity.Property(e => e.DescrizioneCorso).HasColumnName("DESCRIZIONE_CORSO");

                entity.Property(e => e.FileInput).HasColumnName("FILE_INPUT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMERIC");

                entity.Property(e => e.Incarichi).HasColumnName("INCARICHI");

                entity.Property(e => e.Matricola).HasColumnName("MATRICOLA");
            });

            modelBuilder.Entity<Corso0001>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CORSO_0001");

                entity.Property(e => e.Annotazioni).HasColumnName("ANNOTAZIONI");

                entity.Property(e => e.CodNucleo).HasColumnName("COD_NUCLEO");

                entity.Property(e => e.CodReparto).HasColumnName("COD_REPARTO");

                entity.Property(e => e.CodSezione).HasColumnName("COD_SEZIONE");

                entity.Property(e => e.CodUuoo).HasColumnName("COD_UUOO");

                entity.Property(e => e.Cognome).HasColumnName("COGNOME");

                entity.Property(e => e.DataEsecuzione).HasColumnName("DATA_ESECUZIONE");

                entity.Property(e => e.DataPianificazione).HasColumnName("DATA_PIANIFICAZIONE");

                entity.Property(e => e.GradoQualifica).HasColumnName("GRADO_QUALIFICA");

                entity.Property(e => e.IdCorso).HasColumnName("ID_CORSO");

                entity.Property(e => e.IdDipendente).HasColumnName("ID_DIPENDENTE");

                entity.Property(e => e.Matricola).HasColumnName("matricola");

                entity.Property(e => e.MilCiv).HasColumnName("MIL_CIV");

                entity.Property(e => e.Nome).HasColumnName("NOME");

                entity.Property(e => e.Nucleo).HasColumnName("NUCLEO");

                entity.Property(e => e.Reparto).HasColumnName("REPARTO");

                entity.Property(e => e.Sezione).HasColumnName("SEZIONE");
            });

            modelBuilder.Entity<EntiFormatori>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ENTI_FORMATORI");

                entity.Property(e => e.Ente).HasColumnName("ENTE");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<GradoMilitari>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GRADO_MILITARI");

                entity.Property(e => e.Area).HasColumnName("AREA");

                entity.Property(e => e.Grado).HasColumnName("GRADO");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Incarico>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("INCARICO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Incarico1).HasColumnName("INCARICO");
            });

            modelBuilder.Entity<MateriaCorso>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MATERIA_CORSO");

                entity.Property(e => e.CodiceMateriaCorso).HasColumnName("CODICE_MATERIA_CORSO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MateriaCorso1).HasColumnName("MATERIA_CORSO");
            });

            modelBuilder.Entity<Param>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PARAM");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NomeParametro).HasColumnName("NOME_PARAMETRO");

                entity.Property(e => e.ValoreParametro).HasColumnName("VALORE_PARAMETRO");
            });

            modelBuilder.Entity<Personale>(entity =>
            {
                entity.HasKey(e => e.Matricola);

                entity.ToTable("PERSONALE");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Matricola).HasColumnName("MATRICOLA");

                entity.Property(e => e.Annotazioni).HasColumnName("ANNOTAZIONI");

                entity.Property(e => e.CategoriaProfilo).HasColumnName("CATEGORIA_PROFILO");

                entity.Property(e => e.CodNucleo).HasColumnName("COD_NUCLEO");

                entity.Property(e => e.CodReparto).HasColumnName("COD_REPARTO");

                entity.Property(e => e.CodSezione).HasColumnName("COD_SEZIONE");

                entity.Property(e => e.CodUfficio).HasColumnName("COD_UFFICIO");

                entity.Property(e => e.Cognome).HasColumnName("COGNOME");

                entity.Property(e => e.GradoQualifica).HasColumnName("GRADO_QUALIFICA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Incarico).HasColumnName("INCARICO");

                entity.Property(e => e.MilCiv).HasColumnName("MIL_CIV");

                entity.Property(e => e.Nome).HasColumnName("NOME");

                entity.Property(e => e.StatoServizio).HasColumnName("STATO_SERVIZIO");
            });

            modelBuilder.Entity<ProfiloCivili>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PROFILO_CIVILI");

                entity.Property(e => e.Area).HasColumnName("AREA");

                entity.Property(e => e.CodProfilo).HasColumnName("COD_PROFILO");

                entity.Property(e => e.DescProfilo).HasColumnName("DESC_PROFILO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Qualifica).HasColumnName("QUALIFICA");
            });

            modelBuilder.Entity<QualificaCivili>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("QUALIFICA_CIVILI");

                entity.Property(e => e.Area).HasColumnName("AREA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Qualifica).HasColumnName("QUALIFICA");
            });

            modelBuilder.Entity<StatoServizio>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("STATO_SERVIZIO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Stato).HasColumnName("STATO");

                entity.Property(e => e.TipoPersonale).HasColumnName("TIPO_PERSONALE");
            });

            modelBuilder.Entity<Tables>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TABLES");

                entity.Property(e => e.NomeTabella).HasColumnName("NOME_TABELLA");
            });

            modelBuilder.Entity<TipoFormazione>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TIPO_FORMAZIONE");

                entity.Property(e => e.AttivitaFormativa).HasColumnName("ATTIVITA_FORMATIVA");

                entity.Property(e => e.CodiceAttivitaFormativa).HasColumnName("CODICE_ATTIVITA_FORMATIVA");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<TipologiaCorso>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TIPOLOGIA_CORSO");

                entity.Property(e => e.AttivitaFormativa).HasColumnName("ATTIVITA_FORMATIVA");

                entity.Property(e => e.CodiceAttivitaFormativa).HasColumnName("CODICE_ATTIVITA_FORMATIVA");

                entity.Property(e => e.CodiceMateriaCorso).HasColumnName("CODICE_MATERIA_CORSO");

                entity.Property(e => e.CodiceTipologiaCorso).HasColumnName("CODICE_TIPOLOGIA_CORSO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MateriaCorso).HasColumnName("MATERIA_CORSO");

                entity.Property(e => e.TipologiaCorso1).HasColumnName("TIPOLOGIA_CORSO");
            });

            modelBuilder.Entity<To>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TO");

                entity.Property(e => e.CodNucleo).HasColumnName("Cod_Nucleo");

                entity.Property(e => e.CodReparto).HasColumnName("Cod_Reparto");

                entity.Property(e => e.CodSezione).HasColumnName("Cod_Sezione");

                entity.Property(e => e.CodUuoo).HasColumnName("Cod_UUOO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Nucleo).HasColumnName("NUCLEO");

                entity.Property(e => e.Reparto).HasColumnName("REPARTO");

                entity.Property(e => e.Sezione).HasColumnName("SEZIONE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
