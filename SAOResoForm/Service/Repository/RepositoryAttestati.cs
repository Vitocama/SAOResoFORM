using Microsoft.EntityFrameworkCore;
using SAOResoForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAOResoForm.Repositories
{
    public class RepositoryAttestato : IRepositoryAttestato
    {
        #region Create

        public Attestati Add(Attestati attestato)
        {
            using (var db = new tblContext())
            {
                if (attestato == null)
                    throw new ArgumentNullException(nameof(attestato));

                db.Attestati.Add(attestato);
                db.SaveChanges();
                return attestato;
            }
        }

        public List<Attestati> AddRange(List<Attestati> attestati)
        {
            using (var db = new tblContext())
            {
                if (attestati == null)
                    throw new ArgumentNullException(nameof(attestati));

                db.Attestati.AddRange(attestati);
                db.SaveChanges();
                return attestati;
            }
        }

        #endregion

        #region Read

        public Attestati GetById(long id)
        {
            using (var db = new tblContext())
            {
                return db.Attestati.Find(id);
            }
        }

       

      

      

       

        

        public List<Attestati> GetScaduti()
        {
            using (var db = new tblContext())
            {
                var dataOggi = DateTime.Now.ToString("yyyy-MM-dd");

                return db.Attestati
                    .Where(a => !string.IsNullOrEmpty(a.DataScadenzaCorso) &&
                               a.DataScadenzaCorso.CompareTo(dataOggi) < 0)
                    .OrderBy(a => a.DataScadenzaCorso)
                    .ToList();
            }
        }

        public List<Attestati> GetInScadenza(int giorniSoglia = 30)
        {
            using (var db = new tblContext())
            {
                var dataOggi = DateTime.Now.ToString("yyyy-MM-dd");
                var dataSoglia = DateTime.Now.AddDays(giorniSoglia).ToString("yyyy-MM-dd");

                return db.Attestati
                    .Where(a => !string.IsNullOrEmpty(a.DataScadenzaCorso) &&
                               a.DataScadenzaCorso.CompareTo(dataOggi) >= 0 &&
                               a.DataScadenzaCorso.CompareTo(dataSoglia) <= 0)
                    .OrderBy(a => a.DataScadenzaCorso)
                    .ToList();
            }
        }

        #endregion

        #region Update

        public Attestati Update(Attestati attestato)
        {
            using (var db = new tblContext())
            {
                if (attestato == null)
                    throw new ArgumentNullException(nameof(attestato));

                db.Attestati.Update(attestato);
                db.SaveChanges();
                return attestato;
            }
        }

        public List<Attestati> UpdateRange(List<Attestati> attestati)
        {
            using (var db = new tblContext())
            {
                if (attestati == null)
                    throw new ArgumentNullException(nameof(attestati));

                db.Attestati.UpdateRange(attestati);
                db.SaveChanges();
                return attestati;
            }
        }

        #endregion

        #region Delete

       

        #endregion

        #region Utility

        public bool Exists(long id)
        {
            using (var db = new tblContext())
            {
                return db.Attestati.Any(a => a.Id == id);
            }
        }

        public int Count()
        {
            using (var db = new tblContext())
            {
                return db.Attestati.Count();
            }
        }

        public int CountByMatricola(string matricola)
        {
            using (var db = new tblContext())
            {
                if (string.IsNullOrWhiteSpace(matricola))
                    return 0;

                return db.Attestati
                    .Count(a => a.MatricolaDipendente == matricola);
            }
        }

             public Attestati SalvaAttestato(Attestati attestato)
        {
            return Add(attestato);
        }

        public List<Attestati> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Attestati> GetByMatricola(string matricola)
        {
            throw new NotImplementedException();
        }

        public List<Attestati> GetByEnteFormatore(string enteFormatore)
        {
            throw new NotImplementedException();
        }

        public List<Attestati> GetByTitoloCorso(string titoloCorso)
        {
            throw new NotImplementedException();
        }

        public List<Attestati> GetByAnnoCorso(string annoCorso)
        {
            throw new NotImplementedException();
        }
    }







        #endregion
    }
