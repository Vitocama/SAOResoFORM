using SAOResoForm.Models;
using System.Collections.Generic;

namespace SAOResoForm.Service.Repository
{
    public interface IRepositoryService
    {
        // Salva un nuovo personale
        string Save(Personale item);

        // Elimina un personale
        string Delete(int id);

        // Aggiorna un personale esistente
        string Update(Personale item);

        // Ottiene un personale per ID
        Personale GetById(string matricola);

        // Ottiene tutti i personale
        List<Personale> GetAll();

        // Cerca personale per criteri (opzionale)
        List<Personale> Search(string searchTerm);

         void SalvaModifiche();
    }
}