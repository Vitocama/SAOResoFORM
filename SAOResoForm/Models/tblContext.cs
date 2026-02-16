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
    
        public virtual DbSet<Personale> Personale { get; set; }
       

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

                entity.Property(e => e.CodiceAttivitaFormativa).HasColumnName("ATTIVITA_FORMATIVA");

                entity.Property(e => e.CodiceMateriaCorso).HasColumnName("MATERIA_CORSO");

                entity.Property(e => e.Note).HasColumnName("NOTE");

                entity.Property(e => e.DataFineCorso).HasColumnName("DATA_FINE_CORSO");

                entity.Property(e => e.DataInizioCorso).HasColumnName("DATA_INIZIO_CORSO");

               

                entity.Property(e => e.DataScadenzaCorso).HasColumnName("DATA_SCADENZA_CORSO");

                entity.Property(e => e.DenominazioneEnteCertificatore).HasColumnName("DENOMINAZIONE_ENTE_CERTIFICATORE");


                entity.Property(e => e.EnteFormatore).HasColumnName("ENTE_FORMATORE");

                entity.Property(e => e.LinkAttestato).HasColumnName("LINK_ATTESTATO");

                entity.Property(e => e.MatricolaDipendente).HasColumnName("MATRICOLA_DIPENDENTE");

                entity.Property(e => e.TitoloCorso).HasColumnName("TITOLO_CORSO");

                entity.Property(e => e.ValiditaAnni).HasColumnName("VALIDITA_ANNI");

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

                entity.Property(e => e.Attivo).HasColumnName("ATTIVO");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
