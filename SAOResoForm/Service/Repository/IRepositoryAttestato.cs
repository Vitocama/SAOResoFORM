using SAOResoForm.Models;
using System.Collections.Generic;

namespace SAOResoForm.Repositories
{
    
    public interface IRepositoryAttestato
    {
       
        Attestati Add(Attestati attestato);
        List<Attestati> GetAll();
        List<Attestati> GetScaduti();
        List<Attestati> GetInScadenza(int giorniSoglia = 30);
        int CountByMatricola(string matricola);

       
    }
}