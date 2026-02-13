using SAOResoForm.Models;
using System;
using System.Collections.Generic;
using SAOResoForm.Models;
using System;
using System.Linq;
using SAOResoForm.Dati;

namespace SAOResoForm.Service.Repository
{
    public class RepositoryService : IRepositoryService
    {
        public string Save(Personale item)
        {
            try
            {
                using (var context = new tblContext())
                {
                    context.Personale.Add(item);
                    context.SaveChanges();
                    return $"Personale {item.Cognome} {item.Nome} salvato con successo!";
                }
            }
            catch (Exception ex)
            {
                return $"Errore nel salvataggio: {ex.Message}";
            }
        }


        public string Update(Personale item)
        {
            string coduff = string.Join(" - ", new[] { item.CodReparto, item.CodSezione, item.CodNucleo }
     .Where(s => !string.IsNullOrWhiteSpace(s)));

            Cod_UUOO cod_UUOO = new Cod_UUOO();
            string valore = cod_UUOO.reparti[coduff].ToString();

            try
            {
                using (var context = new tblContext())
                {
                    var esistente = context.Personale.Find(item.Matricola);

                    if (esistente != null)  // ← AGGIUNTO CONTROLLO
                    {
                        // Aggiorna i campi
                        esistente.Nome = item.Nome;
                        esistente.Cognome = item.Cognome;
                        esistente.GradoQualifica = item.GradoQualifica;
                        esistente.CategoriaProfilo = item.CategoriaProfilo;
                        esistente.MilCiv = item.MilCiv;
                        esistente.CodReparto = item.CodReparto;
                        esistente.CodSezione = item.CodSezione;
                        esistente.CodNucleo = item.CodNucleo;
                        esistente.CodUfficio = int.Parse(valore);
                        esistente.Incarico = item.Incarico;
                        esistente.StatoServizio = item.StatoServizio;
                        esistente.Annotazioni = item.Annotazioni;

                        context.SaveChanges();
                        return $"Personale {item.Cognome} {item.Nome} aggiornato con successo!";
                    }

                    return $"Personale con Matricola {item.Matricola} non trovato."; // ← CORRETTO
                }
            }
            catch (Exception ex)
            {
                return $"Errore nell'aggiornamento: {ex.Message}";
            }
        }

        public Personale GetById(string matricola)
        {
            try
            {
                using (var context = new tblContext())
                {
                    return context.Personale.Find(matricola);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nel recupero del personale: {ex.Message}");
            }
        }

        public List<Personale> GetAll()
        {
            try
            {
                var db = new tblContext();
                
                    return db.Personale.ToList();
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nel recupero dei dati: {ex.Message}");
            }
        }

        public List<Personale> Search(string searchTerm)
        {
            try
            {
                using (var context = new tblContext())
                {
                    if (string.IsNullOrWhiteSpace(searchTerm))
                    {
                        return context.Personale.ToList();
                    }

                    return context.Personale
                        .Where(p =>
                            p.Nome.Contains(searchTerm) ||
                            p.Cognome.Contains(searchTerm) ||
                            p.Matricola.Contains(searchTerm) ||
                            p.GradoQualifica.Contains(searchTerm))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nella ricerca: {ex.Message}");
            }
        }

        public string Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}