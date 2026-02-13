using SAOResoForm.Models;
using System.Collections.Generic;

namespace SAOResoForm.Repositories
{
    public interface IRepositoryAttestato
    {
        // Create
        Attestati Add(Attestati attestato);

        // Read
        List<Attestati> GetAll();
        List<Attestati> GetScaduti();
        List<Attestati> GetInScadenza(int giorniSoglia = 30);

        // Update
        Attestati Update(Attestati attestato);

        // Utility
        Attestati SalvaAttestato(Attestati attestato);
    }
}