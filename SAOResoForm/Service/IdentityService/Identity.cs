using SAOResoForm.Models;
using System.Linq;

namespace SAOResoForm.Service.IdentityService
{
    public class Identity : IIdentity
    {
        public bool Autenticato(string utente, string password)
        {
             var bd = new tblContext(); // ← using per dispose corretto
           bool autenticato  bd.AccountUtenti.Any(a => a.Utente == utente && a.Password == password);
            
            return autenticato;
        }
    }
}