using SAOResoForm.Models;
using System;
using System.Collections.Generic;
using SAOResoForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public string Delete(int id)
        {
            try
            {
                using (var context = new tblContext())
                {
                    var personale = context.Personale.Find(id);
                    if (personale != null)
                    {
                        context.Personale.Remove(personale);
                        context.SaveChanges();
                        return $"Personale con ID {id} eliminato con successo!";
                    }
                    return $"Personale con ID {id} non trovato.";
                }
            }
            catch (Exception ex)
            {
                return $"Errore nell'eliminazione: {ex.Message}";
            }
        }

        public string Update(Personale item)
        {
            try
            {
                using (var context = new tblContext())
                {
                    var esistente = context.Personale.Find(item.Id);
                    if (esistente != null)
                    {
                        // Aggiorna i campi
                        esistente.Nome = item.Nome;
                        esistente.Cognome = item.Cognome;
                        esistente.Matricola = item.Matricola;
                        esistente.GradoQualifica = item.GradoQualifica;
                        esistente.CategoriaProfilo = item.CategoriaProfilo;
                        esistente.MilCiv = item.MilCiv;
                        esistente.CodReparto = item.CodReparto;
                        esistente.CodSezione = item.CodSezione;
                        esistente.CodNucleo = item.CodNucleo;
                        esistente.CodUfficio = item.CodUfficio;
                        esistente.Incarico = item.Incarico;
                        esistente.StatoServizio = item.StatoServizio;
                        esistente.Annotazioni = item.Annotazioni;

                        context.SaveChanges();
                        return $"Personale {item.Cognome} {item.Nome} aggiornato con successo!";
                    }
                    return $"Personale con ID {item.Id} non trovato.";
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
                using (var context = new tblContext())
                {
                    return context.Personale.ToList();
                }
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
    }
}