using SAOResoForm.Models;
using System.Collections.Generic;

namespace SAOResoForm.Repositories
{
    public interface IRepositoryAttestato
    {
        // Create
        Attestati Add(Attestati attestato);
        List<Attestati> AddRange(List<Attestati> attestati);

        //salva

        Attestati SalvaAttestato(Attestati attestato);

        // Read
        Attestati GetById(long id);
        List<Attestati> GetAll();
        List<Attestati> GetByMatricola(string matricola);
        List<Attestati> GetByEnteFormatore(string enteFormatore);
        List<Attestati> GetByTitoloCorso(string titoloCorso);
        List<Attestati> GetByAnnoCorso(string annoCorso);
        List<Attestati> GetScaduti();
        List<Attestati> GetInScadenza(int giorniSoglia = 30);

        // Update
        Attestati Update(Attestati attestato);
        List<Attestati> UpdateRange(List<Attestati> attestati);

      

        // Utility
        bool Exists(long id);
        int Count();
        int CountByMatricola(string matricola);
    }
}