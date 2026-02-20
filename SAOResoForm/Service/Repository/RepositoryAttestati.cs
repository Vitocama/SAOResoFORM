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

        #endregion

        #region Read

        public List<Attestati> GetAll()
        {
            using (var db = new tblContext())
            {
                return db.Attestati.Where(x => x.Id !=0).ToList();
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

        #endregion

        #region Utility

        public Attestati SalvaAttestato(Attestati attestato)
        {
            return Add(attestato);
        }

        public List<Attestati> getNotVisible()
        {
            using (var db = new tblContext())
            {
                return db.Attestati.Where(x => x.Attivo == true).ToList();
            }
        }

        public List<Attestati> getVisible()
        {
            using (var db = new tblContext())
            {
                return db.Attestati.Where(x => x.Attivo != true).ToList();
            }
        }



        #endregion
    }
}