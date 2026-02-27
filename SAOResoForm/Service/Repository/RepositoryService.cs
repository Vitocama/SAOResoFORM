using SAOResoForm.Dati;
using SAOResoForm.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

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
            string coduff = item.CodReparto + item.CodSezione + item.CodNucleo;

            string valoreCodUfficio = "";

            try
            {
                coduff = int.Parse(coduff).ToString();
                Cod_UUOO cod_UUOO = new Cod_UUOO();
                string reparti = cod_UUOO.reparti.FirstOrDefault(x => x.Value.ToString() == coduff).Key;

                string[] codice = reparti?.Split('-') ?? new string[3];

                item.CodReparto = codice.Length > 0 ? codice[0].Trim() : "";
                item.CodSezione = codice.Length > 1 ? codice[1].Trim() : "";
                item.CodNucleo = codice.Length > 2 ? codice[2].Trim() : "";
            }
            catch {
                coduff = item.CodReparto + " - " + item.CodSezione + " - " + item.CodNucleo;
            }

            coduff = coduff.TrimEnd(' ', '-');

           

            try
            {
                using (var context = new tblContext())
                {
                    var esistente = context.Personale.Find(item.Matricola);

                    if (esistente != null)  // ← AGGIUNTO CONTROLLO
                    {
                        // Aggiorna i campi
                        esistente.Nome = item.Nome.ToUpper();
                        esistente.Cognome = item.Cognome.ToUpper();
                        esistente.GradoQualifica = item.GradoQualifica;
                        esistente.CategoriaProfilo = item.CategoriaProfilo;
                        esistente.MilCiv = item.MilCiv;
                        esistente.CodReparto = item.CodReparto.ToUpper();
                        esistente.CodSezione = item.CodSezione.ToUpper();
                        esistente.CodNucleo = item.CodNucleo.ToUpper();
                        esistente.CodUfficio = long.TryParse(coduff, out long val) ? val : 0; // ← AGGIUNTO CALCOLO COD_UFFICIO
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

        public void SalvaModifiche()
        {
            throw new NotImplementedException();
        }

        public string SaveAccount(AccountUtenti item)
        {
            try
            {
                using (var db = new tblContext())
                {
                    if (db.AccountUtenti.Any(a => a.Utente == item.Utente))
                        return $"L'utente '{item.Utente}' esiste già!";

                    db.AccountUtenti.Add(item);
                    db.SaveChanges();
                    return $"Account '{item.Utente}' creato con successo!";
                }
            }
            catch (Exception ex)
            {
                return $"Errore nel salvataggio: {ex.Message}";
            }
        }
    }
    }
